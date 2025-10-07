using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICarController : MonoBehaviour
{
    [Header("Path & Waypoints")]
    public Transform pathGroup;
    public float distFromPath = 20f;

    [Header("Car Physics & Setup")]
    public Transform centerOfMassObject;
    private Rigidbody rb;

    [Header("Wheel Colliders")]
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    [Header("Car Specs")]
    public float maxSteerAngle = 15f;

    // Internal variables
    private List<Transform> pathNodes = new List<Transform>();
    private int currentNode = 0;

    private CarSpeedController speedController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (centerOfMassObject != null)
        {
            rb.centerOfMass = transform.InverseTransformPoint(centerOfMassObject.position);
        }
        GetPath();
        speedController = GetComponent<CarSpeedController>();
        if (speedController != null)
        {
            speedController.wheelRL = wheelRL;
            speedController.wheelRR = wheelRR;
        }
    }

    void FixedUpdate()
    {
        ApplySteer();
        if (speedController != null)
        {
            speedController.ControlSpeed();
        }
        CheckWaypointDistance();
    }

    // Populates the list of waypoints from the pathGroup object
    void GetPath()
    {
        Transform[] childObjects = pathGroup.GetComponentsInChildren<Transform>();

        // Add all child transforms to the pathNodes list, ignoring the parent itself
        foreach (Transform child in childObjects)
        {
            if (child != pathGroup.transform)
            {
                pathNodes.Add(child);
            }
        }
    }

    // Handles steering logic
    void ApplySteer()
    {
        if (pathNodes.Count == 0) return;

        // Calculate a vector from the car to the current waypoint in the car's local space
        Vector3 steerVector = transform.InverseTransformPoint(pathNodes[currentNode].position);

        // Calculate the new steering angle
        float newSteer = maxSteerAngle * (steerVector.x / steerVector.magnitude);

        // Apply the steering angle to the front wheels
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }

    // ...existing code...

    // Checks distance to the current waypoint and updates to the next one if close enough
    void CheckWaypointDistance()
    {
        if (pathNodes.Count == 0) return;

        float distance = Vector3.Distance(transform.position, pathNodes[currentNode].position);

        if (distance < distFromPath)
        {
            // If we are close to the last waypoint, loop back to the first one
            if (currentNode == pathNodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                // Move to the next waypoint in the list
                currentNode++;
            }
        }
    }
}