using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class should give you access to all the information you need to determine the fitness value of its corresponding individual.
/// 
/// Author: Sascha Schewe
/// </summary>
public class CarState : MonoBehaviour {
    public Transform goalTransform;
    public static Transform spawnPoint;

    private int collisionCount=0;
    private Rigidbody rigid;
    private CarController controller;
    private Renderer carRenderer;
    private Color defaultColor;

    private void Start()
    {
        if (spawnPoint == null)
        {
            spawnPoint = new GameObject("SpawnPoint").transform;
            spawnPoint.position = transform.position;
            spawnPoint.rotation = transform.rotation;
        }
        rigid = GetComponentInChildren<Rigidbody>();
        controller = GetComponent<CarController>();
        carRenderer = GetComponentInChildren<Renderer>();
        defaultColor = carRenderer.material.color;
        goalTransform = GameObject.Find("GoalPosition").transform;
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionCount++;
        carRenderer.material.color = Color.red;
    }

    /// <summary>
    /// Distance between the desired final location and current location
    /// </summary>
    /// <returns>distance</returns>
    public float DistanceFromGoal()
    {
        return (transform.position - goalTransform.position).sqrMagnitude;
    }

    /// <summary>
    /// Disparity between desired final angle and current real angle
    /// </summary>
    /// <returns>~0-4, 0 being ideal</returns>
    public float AngleToGoal()
    {
        return (transform.TransformDirection(transform.forward) - goalTransform.TransformDirection(goalTransform.forward)).sqrMagnitude;
    }

    /// <summary>
    /// Number of collisions since the last reset
    /// </summary>
    /// <returns>collision count</returns>
    public int NumberOfCollisions()
    {
        return collisionCount;
    }

    /// <summary>
    /// Returns the car's current velocity
    /// </summary>
    /// <returns>velocity</returns>
    public float CurrentVelocity()
    {
        return rigid.velocity.sqrMagnitude;
    }

    /// <summary>
    /// Resets the car
    /// </summary>
    public void Reset()
    {
        collisionCount = 0;
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        controller.ApplyBrakes();
        carRenderer.material.color = defaultColor;
    }
}
