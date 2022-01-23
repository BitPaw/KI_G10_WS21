using UnityEngine;

class QuickMaths
{
    static public void SpreadedRayCast(Transform ghostTransform, int amountOfRays, ref float[] outputDataCache, bool drawRays)
    {
        int fieldOfView = 360;
        int sensorDataIndex = 0;
        int amountOfSteps = fieldOfView / amountOfRays;
        Quaternion originalRotation = ghostTransform.rotation;

        for (int step = 0; step < fieldOfView; step += amountOfSteps) // Robot-Sensor loop
        {
            float maxRange = float.PositiveInfinity;
            Vector3 ghostPosition = ghostTransform.position;
            float posRR = ((fieldOfView / amountOfRays * 2) - (fieldOfView - step - (amountOfSteps))) * 0.5f;

            ghostTransform.Rotate(0, posRR, 0);

            Vector3 ghostDirection = ghostTransform.forward;
            RaycastHit raycastHit;
            bool hasHit = Physics.Raycast(ghostPosition, ghostDirection, out raycastHit, maxRange);
            float distanceToWall = float.PositiveInfinity;

            if (hasHit)
            {
                distanceToWall = Vector3.Distance(raycastHit.point, ghostTransform.position);

                if (drawRays)
                {
                    ghostDirection *= distanceToWall;

                    Debug.DrawRay(ghostPosition, ghostDirection, Color.cyan);
                }
            }
            else
            {
                if (drawRays)
                {
                    ghostDirection *= 100;

                    Debug.DrawRay(ghostPosition, ghostDirection, Color.red);
                }
            }

            outputDataCache[sensorDataIndex++] = distanceToWall;

            ghostTransform.rotation = originalRotation;
        }
    }

    static public float Average(float[] values, int length)
    {
        int i = 0;
        float sum = 0;

        for (; i < length; i++)
        {
            sum += values[i];
        }

        return sum / i;
    }

    static public float StandardDeviationCalculate(float[] values, int length, float avrage)
    {
        float amount = 0;
        float sum = 0;

        for (int i = 0; i < length; i++)
        {
            if (values[i] == float.PositiveInfinity)
            {
                continue;
            }

            sum += Mathf.Pow(values[i] - avrage, 2);
            ++amount;
        }

        float rootBase = sum / (float)amount;
        float result = Mathf.Sqrt(rootBase);

        return result;
    }

    // z = (x - y) / o
    //
    // z = ?
    // x = observed value
    // y = Avearge of all values 
    // o = standard deviation
    static public float NormalDistribution(float x, float y, float o)
    {
        return (x - y) / o;
    }
}