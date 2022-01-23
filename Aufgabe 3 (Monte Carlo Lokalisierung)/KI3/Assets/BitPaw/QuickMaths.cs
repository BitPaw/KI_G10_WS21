using UnityEngine;

class QuickMaths
{
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