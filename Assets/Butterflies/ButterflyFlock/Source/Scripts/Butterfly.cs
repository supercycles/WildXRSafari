using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour
{
    // The states of the Butterfly
    public enum ButterflyStatus
    {
        STARTING_TO_FLY,
        FLYING,
        LANDING,
        RESTING,
    }

    // The flock it belongs to.
    [HideInInspector]
    public ButterflyFlock _flock;
    // Can it move?
    [HideInInspector]
    public bool canMove = true;

    [Tooltip("Enable avoidance for this Butterfly")]
    public bool enableAvoidance = true;

    [Tooltip("The current status of the Butterfly. (Read-only)")]
    public ButterflyStatus status = ButterflyStatus.STARTING_TO_FLY;

    //[Tooltip("The mesh renderer of the butterfly. (REQUIRED)")]
    //public MeshRenderer skinnedMeshRenderer;

    [Tooltip("Offset for the position when resting.")]
    public Vector3 restingPositionOffset;
    [Tooltip("Offset for the rotation when resting.")]
    public Vector3 restingRotationOffset;

    // Target speed for the butterfly (Read-only)
    [HideInInspector]
    public float _targetSpeed;

    // Target rotation speed for the butterfly (Read-only)
    [HideInInspector]
    public float _targetRotationSpeed;

    // Target position of the butterfly (Read-only)
    [HideInInspector]
    public Vector3 _targetPosition;

    // Public access to the butterfly current speed
    public float Speed => _speed;
    
    float _speed;

    void Start()
    {

        status = ButterflyStatus.STARTING_TO_FLY;

        enableAvoidance = _flock.enableAvoidance;

        float _scale = Random.Range(_flock.minMaxScale.x, _flock.minMaxScale.y);
        transform.localScale = Vector3.one * _scale;

        transform.position = GetNewPosition();
        _targetPosition = GetNewPosition();

        _targetSpeed = Random.Range(_flock.minMaxSpeed.x, _flock.minMaxSpeed.y);
        _targetRotationSpeed = Random.Range(_flock.minMaxRotationSpeed.x, _flock.minMaxRotationSpeed.y);
    }


    void Update()
    {
        _speed = Mathf.Lerp(_speed, _targetSpeed, Time.deltaTime * 2.5f);
        if (canMove)
        {
            GetComponentInChildren<Animator>().SetBool("Landed", false);

            if ((transform.position - _targetPosition).magnitude < _flock.minDistanceToPosition)
            {
                _targetSpeed = Random.Range(_flock.minMaxSpeed.x, _flock.minMaxSpeed.y);
                _targetPosition = GetNewPosition();

                status = ButterflyStatus.FLYING;
            }

            transform.position += transform.forward * _speed * Time.deltaTime;// _system._newDelta;
            if (enableAvoidance)
                AvoidObstacles();
        }
    }

    private void LateUpdate()
    {
        if (canMove)
        {
            Vector3 targetRotation = _targetPosition - transform.position;
            if (_targetSpeed >= 0 && targetRotation != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(targetRotation);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _targetRotationSpeed);
            }
        }
    }

    void AvoidObstacles()
    {
        float _randomOffsetLength = Random.Range(.4f, .1f);
        Vector3 forward = transform.forward;
        Quaternion quat = transform.rotation;
        Vector3 currentAngle = quat.eulerAngles;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, forward + (transform.right * -_randomOffsetLength), out hit, _flock.avoidanceRayLength, _flock.avoidanceMask))
        {
            currentAngle.y += _flock.avoidanceForce.x * Time.deltaTime * _targetRotationSpeed;
            quat.eulerAngles = currentAngle;
            transform.rotation = quat;
        }
        else if (Physics.Raycast(transform.position, forward + (transform.right * _randomOffsetLength), out hit, _flock.avoidanceRayLength, _flock.avoidanceMask))
        {
            currentAngle.y -= _flock.avoidanceForce.x * Time.deltaTime * _targetRotationSpeed;
            quat.eulerAngles = currentAngle;
            transform.rotation = quat;
        }
    }

    public Vector3 GetNewPosition()
    {
        var systemPos = _flock.transform.position;
        Vector3 t = Vector3.zero;
        t.x = Random.Range(-_flock.spawnBounds.x, _flock.spawnBounds.x) + systemPos.x;
        t.z = Random.Range(-_flock.spawnBounds.z, _flock.spawnBounds.z) + systemPos.z;
        t.y = Random.Range(-_flock.spawnBounds.y, _flock.spawnBounds.y) + systemPos.y;
        
        //StartCoroutine(UpdatePosition());
        return t;
    }

    IEnumerator UpdatePosition()
    {
        yield return new WaitForSeconds(10);
        if (transform.position != _targetPosition)
        {
            _targetPosition = GetNewPosition();
        }
    }

}
