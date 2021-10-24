using System.Collections.Generic;
using UnityEngine;

namespace GA.Selectors
{
    public class SelectorRandomBitPaw : ISelector
    {
        public List<string> SelectFromGeneration(GenerationDB.Generation parentGeneration)
        {
            List<string> nextGeneration = new List<string>();
            int requiredExtraMutants = parentGeneration.individuals.Count;

            foreach (Individual individual in parentGeneration.individuals)
            {
                bool shouldAdd = Random.value % 2 == 0;

                if (shouldAdd)
                {
                    nextGeneration.Add(individual.GeneSequence);
                    --requiredExtraMutants;
                }
            }

            // We need additional individuals to make a generation big enogh!
            // So we add some that arent as good.
            foreach (Individual individual in parentGeneration.individuals)
            {
                bool shouldAdd = Random.value % 2 == 0;

                if(requiredExtraMutants-- == 0)
                {
                    break;
                }

                if (!shouldAdd)
                {
                    nextGeneration.Add(individual.GeneSequence);
                }
            }

            return nextGeneration;
        }
    }
}
