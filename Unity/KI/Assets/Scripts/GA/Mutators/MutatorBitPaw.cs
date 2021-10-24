using System.Collections.Generic;
using UnityEngine;

namespace GA.Mutators
{
    public class MutatorBitPaw : IMutator
    {
        private List<char> geneIDs = new List<char>();

        public bool UseMutation { get; set; } = true;
        public float MutationChangse { get; set; } = 0.1f;

        public void AssignGene(char geneID)
        {
            geneIDs.Add(geneID);
        }

        public string Mutate(string original)
        {
            int length = original.Length;
            char[] result = new char[length];

            for (int geneIndex = 0; geneIndex < length; geneIndex++)
            {
                char mutatedGene = original[geneIndex];                    

                if (UseMutation)
                {
                    bool mutate = Random.value <= MutationChangse; 

                    if(mutate)
                    {
                        int randomIndex = Random.Range(0, geneIDs.Count);

                        mutatedGene = geneIDs[randomIndex];
                    }                   
                }

                result[geneIndex] = mutatedGene;
            }

            return new string(result);
        }
    }
}
