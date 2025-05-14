using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyFlock : MonoBehaviour
{
    [Header("Flock Settings")]
    [Tooltip("Prefab will be used to spawn the instances.")]
    public Butterfly prefab0;
    public Butterfly prefab1;
    public Butterfly prefab2;
    public Butterfly prefab3;
    public Butterfly prefab4;
    public Butterfly prefab5;
    [Tooltip("Amount of instances to be created.")]
    public int amount = 200;

    [Tooltip("Minimal distance to reach before changing positiong course.")]
    public float minDistanceToPosition = 1f;
    [Tooltip("The spawn bounds of the butteflies as well their available area to navigate.")]
    public Vector3 spawnBounds = new Vector3(2f, 2f, 2f);

    [Tooltip("Speed variance for the instances.")]
    public Vector2 minMaxSpeed = new Vector2(4f, 10f);
    [Tooltip("Rotation speed variance for the instances.")]
    public Vector2 minMaxRotationSpeed = new Vector2(1f, 2f);
    [Tooltip("Scale variance for the instances.")]
    public Vector2 minMaxScale = new Vector2(0.8f, 1f);

    [Tooltip("Draw gizmos for the spawn bounds.")]
    public bool drawGizmosBounds = false;

    [Header("Resting")]
    [Tooltip("List of spots for the butterflies to rest.")]
    public List<RestingSpot> restingSpots;
    [Tooltip("How long it takes for a spot to catch a butterfly so it can rest.")]
    public Vector2Int restDelay = new Vector2Int(1, 4);
    [Tooltip("How long the butterfly will be resting.")]
    public Vector2Int restTime = new Vector2Int(5, 20);
    [Tooltip("Min and Max distance that a butterfly should be from a spot to be chosen to rest.")]
    public Vector2 restMinMaxDistance = new Vector2(5f, 10f);
    [Tooltip("Snap the butterfly into the spot position?")]
    public bool snapIntoSpot = true;

    [Header("Avoidance")]
    [Tooltip("Enable avoidance for the butterflies.")]
    public bool enableAvoidance = true;
    [Tooltip("The length of the ray to be casted when checking for collisions.")]
    public float avoidanceRayLength = 5f;
    [Tooltip("Mask collision to be avoided.")]
    public LayerMask avoidanceMask;
    [Tooltip("The force amount to apply on the butterfly once it collides with a collision.")]
    public Vector2 avoidanceForce = new Vector2(800f, 500f);

    // Public access to the instances
    public List<Butterfly> Instances => _instances;

    List<Butterfly> _instances = new List<Butterfly>();

    void Awake()
    {
        // Initialize instances and create them
        _instances = new List<Butterfly>();
        CreateInstances();

        // Setup the resting spots for the butterflies
        foreach (var restSpot in restingSpots)
        {
            restSpot.flock = this;
        }
    }

    void CreateInstances()
    {
        //Creating instances
        for (int i = 0; i < amount; i++)
        {
            Butterfly obj;
            int num = Random.Range(0, 6);
            switch (num)
            {
                case 0:
                    obj = Instantiate(prefab0);
                    obj._flock = this;
                    _instances.Add(obj);
                    break;
                case 1:
                    obj = Instantiate(prefab1);
                    obj._flock = this;
                    _instances.Add(obj);
                    break;
                case 2:
                    obj = Instantiate(prefab2);
                    obj._flock = this;
                    _instances.Add(obj);
                    break;
                case 3:
                    obj = Instantiate(prefab3);
                    obj._flock = this;
                    _instances.Add(obj);
                    break;
                case 4:
                    obj = Instantiate(prefab4);
                    obj._flock = this;
                    _instances.Add(obj);
                    break;
                case 5:
                    obj = Instantiate(prefab5);
                    obj._flock = this;
                    _instances.Add(obj);
                    break;
            }
        }   
    }

    private void OnDrawGizmos()
    {
        // Draw gizmos for guidance
        if (drawGizmosBounds)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, spawnBounds);
        }
    }
}
