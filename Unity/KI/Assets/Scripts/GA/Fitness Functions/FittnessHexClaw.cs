namespace GA.Fitness_Functions
{
    public class FittnessHexClaw : IFitnessFunction
    {
        public float DetermineFitness(CarState state)
        {
            int distanceMultiplicator = 450;
            int angleMultiplicator = 550;

            float distanceRaw = 1 - UnityEngine.Mathf.Abs(state.DistanceFromGoal());
            float angleRaw = 1 - UnityEngine.Mathf.Abs(state.AngleToGoal());

            float distance = distanceRaw * distanceMultiplicator;
            float angle = angleRaw * angleMultiplicator;

            float collisions = (float)(1 + state.NumberOfCollisions());

            return ((distance + angle) / collisions);
        }
    }
}
