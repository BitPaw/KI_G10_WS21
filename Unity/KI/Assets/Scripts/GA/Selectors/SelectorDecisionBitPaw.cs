using GA.Recombiners;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GA.Selectors
{
    class SelectorDecisionBitPaw : ISelector
    {
        /// <summary>
        /// 1.0f = Take All<para/>
        /// 0.5f = Take Half<para/>
        /// 0.2f = Take Top 20%<para/>
        /// 0.0f = Take none<para/>
        /// </summary>
        public float FitnessThreshhold { get; set; } = 0.66f;

        public List<string> SelectFromGeneration(GenerationDB.Generation parentGeneration)
        {
            int amountOfIndividuals = parentGeneration.individuals.Count;
            int amountOfIndividualsToTake = (int)Math.Floor(amountOfIndividuals * FitnessThreshhold);
            int amountOfIndividualsMissing = amountOfIndividuals - amountOfIndividualsToTake;
            List<string> nextGeneration = new List<string>(amountOfIndividualsToTake);           
           
            parentGeneration.Sort();

            for (int i = 0; i < amountOfIndividualsToTake; i++)
            {
                Individual individual = parentGeneration.Individuals[i];

                nextGeneration.Add(individual.GeneSequence);
            }

            for (int index = 0; (index < amountOfIndividuals) && (amountOfIndividualsMissing-- > 0); )
            {
                Individual individualA = parentGeneration.Individuals[index++];
                Individual individualB = parentGeneration.Individuals[index++];

                string mergedSequence = RecombinerWeaveBitPaw.CombineSequence(individualA.GeneSequence, individualB.GeneSequence);

                nextGeneration.Add(mergedSequence);
            }

            if(nextGeneration.Count != amountOfIndividuals)
            {
                Debug.LogError("[SelectorDecisionBitPaw] Next generation has fewer elements than parent!");
            }          

            return nextGeneration;
        }
    }
}