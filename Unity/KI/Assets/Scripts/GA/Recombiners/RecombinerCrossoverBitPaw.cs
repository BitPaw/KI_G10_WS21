using System;

namespace GA.Recombiners
{
    public class RecombinerCrossoverBitPaw : IRecombiner
    {
        public float TurnPoint { get; set; } = 0.8f;

        public string Combine(string parentA, string parentB)
        {
            int length = parentA.Length;
            char[] result = new char[length];
            int indexTurnPoint = (int)Math.Floor(length * TurnPoint);

            // MEMCPY()?

            for (int i = 0; i < indexTurnPoint; i++)
            {
                result[i] = parentA[i];
            }

            for (int i = indexTurnPoint; i < length; i++)
            {
                result[i] = parentB[i];
            }

            return new string(result);
        }
    }
}
