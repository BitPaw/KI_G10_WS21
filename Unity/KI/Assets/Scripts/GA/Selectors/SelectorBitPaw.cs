using System.Collections.Generic;

namespace GA.Selectors
{
    class SelectorBitPaw : ISelector
    {
        public float FitnessThreshhold { get; set; } = 0.5f;

        public List<string> SelectFromGeneration(GenerationDB.Generation parentGeneration)
        {
            List<string> superAwesomeNewGen = new List<string>();
            Individual bestScore = parentGeneration.Fittest;
            Individual worstScore = parentGeneration.;
            float currentMaximumScore = bestScore.Fitness;
            float currentMinimumScore = 0;

            foreach (Individual ind in parentGeneration.Individuals)
            {
                bool shouldBeSelected = ind.Fitness >= FitnessThreshhold;

                if (shouldBeSelected)
                {
                    superAwesomeNewGen.Add(ind.GeneSequence);
                }
            }

            return superAwesomeNewGen;
        }
    }
}