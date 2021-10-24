using System;
using System.Collections.Generic;

namespace GA.Selectors
{
    class SelectorBitPaw : ISelector
    {
        /// <summary>
        /// 1.0f = Take All<para/>
        /// 0.5f = Take Half<para/>
        /// 0.2f = Take Top 20%<para/>
        /// 0.0f = Take none<para/>
        /// </summary>
        public float FitnessThreshhold { get; set; } = 0.5f;

        public List<string> SelectFromGeneration(GenerationDB.Generation parentGeneration)
        {
            int amountOfIndividuals = parentGeneration.individuals.Count;
            int amountOfIndividualsToTake = (int)Math.Floor(amountOfIndividuals * FitnessThreshhold);
            List<string> nextGeneration = new List<string>(amountOfIndividualsToTake);           
           
            parentGeneration.Sort();

            for (int i = 0; i < amountOfIndividualsToTake; i++)
            {
                Individual individual = parentGeneration.Individuals[i];

                nextGeneration.Add(individual.GeneSequence);
            }

            return nextGeneration;
        }
    }
}