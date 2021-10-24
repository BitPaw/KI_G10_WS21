using UnityEngine;

namespace GA.Terminators
{
    public class TerminatorBitPaw : ITerminator
    {
        public float TargetedRating { get; set; } = 0;
        public float TargetedRatingBuffer { get; set; } = 30;

        public bool JudgementDay(GenerationDB.Generation generation)
        {
            int amountOfindividuals = generation.individuals.Count;

            if(amountOfindividuals <= 0)
            {
                return false; // No Data, No Solution
            }

            bool isFinished = Mathf.Abs(generation.Fittest.Fitness) <= TargetedRating + TargetedRatingBuffer;

            return isFinished;
        }
    }
}
