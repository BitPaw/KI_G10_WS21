using System.Collections.Generic;
using UnityEngine;

namespace GA.Initializers
{
    public class InitializerBitPaw : IInitializer
    {
        private List<char> _geneIDList = new List<char>();


        public float ForwardChangsePercantage { get; set; } = 0.45f;

        public void AssignGene(char geneID)
        {
            _geneIDList.Add(geneID);
        }

        public List<Individual> CreateInitialGeneration(int generationSize, int individualSize)
        {
            List<Individual> individualList = new List<Individual>(generationSize);
            char[] generatedSequence = new char[individualSize];

            for (int index = 0; index < generationSize; index++)
            {
                Individual individual = new Individual();

                for (int i = 0; i < individualSize; i++)
                {
                    bool forward = Random.value < ForwardChangsePercantage;
                    char gene = forward  ? 'B' : 'C'; // A=Reverse, B=Forward, C=Left, D=Right, E=Break

                    generatedSequence[i] = gene;
                }

                individual.GeneSequence = new string(generatedSequence);

                individualList.Add(individual);
            }

            return individualList;
        }
    }
}
