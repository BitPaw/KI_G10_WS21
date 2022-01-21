using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] private float _distanceToWall = 0;
    [SerializeField] private Renderer _renderer = null;

    void Start()
    {
        ControllScript.GetInstance().RegisterGhost(this);
        _renderer = GetComponent<Renderer>();
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
        _distanceToWall = float.PositiveInfinity;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _distanceToWall))
        {         
            _distanceToWall = Vector3.Distance(hit.point, transform.position);

            bool enableDirectionDraw = false;

            if (enableDirectionDraw)
            {
                if (_distanceToWall != float.PositiveInfinity)
                {
                    Debug.DrawRay(transform.position, transform.forward * _distanceToWall, Color.cyan);
                }
                else
                {
                    Debug.DrawRay(transform.position, transform.forward, Color.red);
                }
            }            
        }

        return _distanceToWall;
    }

    public void Move(float distance)
    {
        transform.Translate(0, 0, distance);
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
        _renderer.material.color = color;
        _renderer.material.SetColor("_EmissionColor", color);
    }
}