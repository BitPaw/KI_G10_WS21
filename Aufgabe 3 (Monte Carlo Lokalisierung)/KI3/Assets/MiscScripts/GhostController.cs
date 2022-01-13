﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    void Start()
    {
        ControllScript.GetInstance().RegisterGhost(this);
    }

    void OnDestroy()
    {
        try
        {
            ControllScript.GetInstance().DeRegisterGhost(this);
        }
        catch (System.Exception e)
        {
        }
    }

    public float GetDistance()
    {
        RaycastHit hit;
        float distance = float.PositiveInfinity;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance))
        {
            distance = Vector3.Distance(hit.point, transform.position);
        }

        return distance;
    }

    public void Move(float distance)
    {
        transform.eulerAngles /= 2;
        transform.Translate(transform.forward * distance);
        transform.eulerAngles *= 2;
    }

    public void Rotate(float angle)
    {
        transform.Rotate(0, angle, 0);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Vector3 GetRotation()
    {
        return transform.rotation.eulerAngles;
    }

    public void ChangeColor(Color color)
    {
        gameObject.GetComponent<Renderer>().material.color = color;
    }
}