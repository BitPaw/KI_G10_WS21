using UnityEngine;

namespace GA.Fitness_Functions
{
    public class FittnessHexClaw : IFitnessFunction
    {
        public float DetermineFitness(CarState state)
        {
            const int distanceMultiplicator = 450;
            const int angleMultiplicator = 550;

            float distanceRaw = 1 - Mathf.Abs(state.DistanceFromGoal());
            float angleRaw = 1 - Mathf.Abs(state.AngleToGoal());

            float distance = distanceRaw * distanceMultiplicator;
            float angle = angleRaw * angleMultiplicator;

            float collisions = 1 + state.NumberOfCollisions();

            return ((distance + angle) / collisions);
        }
    }
}
