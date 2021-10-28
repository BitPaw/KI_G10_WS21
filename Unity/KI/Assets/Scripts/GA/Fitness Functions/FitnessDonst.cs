using UnityEngine;

namespace GA.Fitness_Functions
{
    public class FitnessDonst : IFitnessFunction
    {
        public float DetermineFitness(CarState state)
        {
            const int distanceMultiplicator = 450;
            const int angleMultiplicator = 550;

            float fitness = Evaluate(state.DistanceFromGoal(), distanceMultiplicator) +
                          Evaluate(state.AngleToGoal(), angleMultiplicator);

            return (state.NumberOfCollisions() >= 1) ? 
                0f : NormalizeValue(fitness, -1000, 1000);
        }

        private float Evaluate(float value, int multiplicator)
        {
            return (1 - Mathf.Abs(value)) * multiplicator;
        }

        private float NormalizeValue(float value, float minValue, float maxValue)
        {
            return (value - minValue) / (maxValue - minValue);
        }
    }
}