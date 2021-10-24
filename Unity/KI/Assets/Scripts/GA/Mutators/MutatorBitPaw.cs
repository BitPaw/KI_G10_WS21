using System;
using System.Collections.Generic;
using UnityEngine;

namespace GA.Mutators
{
    public class MutatorBitPaw : IMutator
    {
        private List<char> geneIDs = new List<char>();

        public bool UseMutation { get; set; }
        public float MutationThreshhold { get; set; }

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
                char geneA = original[geneIndex];
                char geneB = geneIDs[geneIndex];

                if(UseMutation)
                {
                    float changse = UnityEngine.Random.value;
                    bool mutate = changse >= MutationThreshhold;

                    if(mutate)
                    {
                        result[geneIndex] = geneB;
                    }                   
                }
                else
                {
                    result[geneIndex] = geneA;
                }                
            }

            return new string(result);
        }
    }
}
