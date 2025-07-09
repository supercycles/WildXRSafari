using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestingSpot : MonoBehaviour
{
    [Tooltip("Is the spot taken by a butterfly or not? (Read-only)")]
    public bool taken;
    [Tooltip("Gizmos size to be drawn of the sposts.")]
    public float gizmoSize = 0.1f;
    [Tooltip("Draw gizmos for the spot.")]
    public bool drawGizmos = true;

    // The chosen butterfly to rest
    [HideInInspector]
    public Butterfly targetButterly;
    // The flock that it belongs to
    [HideInInspector]
    public ButterflyFlock flock;

    bool landed; // Did the butterfly landed?
    float restingSpeedMod = 0.85f; // Rest speed modifier for when the butterfly reaches close to the rest spot.

    private void Start()
    {
        if (flock == null) return;

        StartCoroutine(FindAnotherTarget(Random.Range(flock.restDelay.x, flock.restDelay.y)));
    }

    IEnumerator FindAnotherTarget(float delay)
    {
        yield return new WaitForSeconds(delay);
        Release();
        yield return null;
        FindTarget();
    }

    void FindTarget()
    {
        Butterfly foundButterfly = null;
        var butterflies = flock.Instances;
        foreach(var b in butterflies)
        {
            float distance = Vector3.Distance(b.transform.position, transform.position);
            if(b.status == Butterfly.ButterflyStatus.FLYING && distance >= flock.restMinMaxDistance.x && distance <= flock.restMinMaxDistance.y)
            {
                foundButterfly = b;
                break;
            }
        }

        targetButterly = foundButterfly;
        landed = false;

        if (targetButterly == null)
            StartCoroutine(FindAnotherTarget(Random.Range(flock.restDelay.x, flock.restDelay.y)));
        else
        {
            taken = true;
            targetButterly.status = Butterfly.ButterflyStatus.LANDING;
        }
    }

    void LateUpdate()
    {
        if (targetButterly != null)
        {
            if(landed)
            {
                targetButterly._targetPosition = transform.position + targetButterly.restingPositionOffset;

                UpdateRotation();

                targetButterly.GetComponentInChildren<Animator>().SetBool("Landed", true);

                //float currentAnimTimeScale = targetButterly.skinnedMeshRenderer.material.GetFloat("_WingScale");
                //if(currentAnimTimeScale > 0.1f)
                //{
                //    currentAnimTimeScale = Mathf.MoveTowards(currentAnimTimeScale, 0.2f, Time.deltaTime);
                //    targetButterly.skinnedMeshRenderer.material.SetFloat("_WingScale", currentAnimTimeScale);
                //}

                return;
            }

            targetButterly.status = Butterfly.ButterflyStatus.LANDING;

            float distance = Vector3.Distance(targetButterly.transform.position, transform.position + targetButterly.restingPositionOffset);
            if(distance < 5 && distance > .5f)
            {
                landed = false;

                targetButterly._targetSpeed = /*flock.minMaxSpeed.y * */restingSpeedMod;
                targetButterly._targetPosition = transform.position + targetButterly.restingPositionOffset;
                targetButterly.enableAvoidance = false;
            }
            else if(distance <= .5f)
            {
                targetButterly._targetPosition = transform.position + targetButterly.restingPositionOffset;

                //snap land distance
                if(distance > 0.1f)
                {
                    targetButterly._targetSpeed = flock.minMaxSpeed.x * restingSpeedMod;
                    targetButterly.transform.position += (transform.position + targetButterly.restingPositionOffset - targetButterly.transform.position) 
                        * Time.deltaTime * targetButterly.Speed * (restingSpeedMod) * 2;
                }
                else
                {
                    landed = true;
                    targetButterly.status = Butterfly.ButterflyStatus.RESTING;

                    if (flock.snapIntoSpot)
                    {
                        targetButterly.transform.position = transform.position;
                    }

                    StartCoroutine(FindAnotherTarget(Random.Range(targetButterly._flock.restTime.x, targetButterly._flock.restTime.y)));
                }

                targetButterly.canMove = false;
                UpdateRotation();
                
            }
            else
            {
                targetButterly._targetPosition = transform.position + targetButterly.restingPositionOffset;
            }


        }
    }

    void UpdateRotation()
    {
        Quaternion rotation = transform.rotation;
        rotation.eulerAngles += targetButterly.restingRotationOffset;
        targetButterly.transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * targetButterly._targetRotationSpeed * restingSpeedMod);
    }

    public void Release()
    {
        if (targetButterly != null)
        {
            //targetButterly.skinnedMeshRenderer.material.SetFloat("_WingScale", 1f);
            targetButterly._targetPosition = transform.position + targetButterly.restingPositionOffset;
            targetButterly.canMove = true;
            targetButterly.enableAvoidance = flock.enableAvoidance;
            targetButterly.status = Butterfly.ButterflyStatus.STARTING_TO_FLY;

            targetButterly = null;
            taken = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, gizmoSize);
        }
    }
}

