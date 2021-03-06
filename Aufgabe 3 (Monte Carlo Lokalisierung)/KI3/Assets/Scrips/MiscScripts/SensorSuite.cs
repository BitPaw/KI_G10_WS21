using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorSuite : MonoBehaviour
{
    [SerializeField]  public float Range = 2f, DeflectionAngle = 45f, ProbOfFailedScan = .2f, Wonkyness = .4f;
    [SerializeField]  public int WonkynessDistributionDampening = 5;
    [SerializeField]  public bool ScansDeflect = true, FuzzyifyDistances = true, ScansCanFail = true, CanCorrectFailedScans = false;
    [SerializeField]  private float spread = 5f, degreesPerStep, minDistance;
    [SerializeField]  private int steps = 10;


    // Start is called before the first frame update
    void Start()
    {
        AdjustSpread();
    }

    void Update()
    {
        GetDistance();
    }

    public float GetDistance()
    {
        float minDistance = Range;
        if((ScansCanFail && Random.value < ProbOfFailedScan) || CheckFrontColission())
        {
            return minDistance;
        }
        minDistance = SingleScan();
        float tmp;
        transform.Rotate(0, -spread / 2f, 0);
        for(int i = 0; i < steps; i++)
        {
            tmp = SingleScan();
            if (minDistance > tmp)
                minDistance = tmp;
            transform.Rotate(0, degreesPerStep, 0);
        }
        tmp = SingleScan();
        if (minDistance > tmp)
            minDistance = tmp;
        transform.Rotate(0, -spread / 2f, 0);
        if (FuzzyifyDistances)
        {
            minDistance = Fuzzify(minDistance);
        }
        return minDistance;
    }

    private float Fuzzify(float distance)
    {
        float adjustment=0;
        for(int i = 0; i < WonkynessDistributionDampening; i++)
        {
            adjustment += Random.value * Wonkyness * 2;
        }
        adjustment /= WonkynessDistributionDampening;
        adjustment *= distance;
        distance *= 1 - Wonkyness;
        distance += adjustment;
        return distance;
    }

    private float SingleScan()
    {
        RaycastHit hit;
        minDistance = Range;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Range))
        {
            Debug.DrawLine(transform.position, hit.point);
            float distance = Vector3.Distance(hit.point, transform.position);
            if (ScansDeflect)
            {
                if (Vector3.Angle(hit.normal, -transform.forward) > DeflectionAngle)
                {
                    distance = Range;
                }
            }
            if (distance < minDistance)
                minDistance = distance;
        }
        return minDistance;
    }

    private void AdjustSpread()
    {
        degreesPerStep = spread / (float)steps;
    }

    public float Spread
    {
        get
        {
            return spread;
        }
        set
        {
            spread = value;
            AdjustSpread();
        }
    }
    public int Steps
    {
        get
        {
            return steps;
        }
        set
        {
            steps = value;
            AdjustSpread();
        }
    }
    private bool CheckFrontColission()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, .2f))
        {
            return true;
        }
        return false;
    }
}
