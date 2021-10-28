using System;
using System.Collections.Generic;
using System.Text;

namespace GA.Initializers
{
    public class InitializerDonst : IInitializer
    {
        private List<char> _geneIDList = new List<char>();
        private Random random = new Random();

        public void AssignGene(char ID)
        {
            _geneIDList.Add(ID);
        }

        public List<Individual> CreateInitialGeneration(int generationSize, int individualSize)
        {
            List<Individual> generations = new List<Individual>();
            Individual individual;

            for (int i = 0; i < generationSize; i++)
            {
                individual = new Individual();
                individual.geneSequence = GetRandomGeneSequence(individualSize);
                generations.Add(individual);
            }

            return generations;
        }

        private string GetRandomGeneSequence(int size)
        {
            string sequence = "";

            for (int i = 0; i < size; i++)
                sequence += GetRandomGene();

            sequence = ModifyWithBrakes(sequence);

            return sequence;
        }

        private string ModifyWithBrakes(string sequence)
        {
            int lastIndex = sequence.Length - 1;
            int lengthOfTail = (int) Math.Round(sequence.Length * 0.15);
            string tail = "";

            for (int i = 0; i < lengthOfTail; i++)
                tail += "E";

            return sequence.Substring(0, lastIndex - lengthOfTail) + tail;
        }

        private char GetRandomGene()
        {
            switch (random.Next(9))
            {
                case 0:
                case 1:
                case 2:
                    return 'B';
                case 3:
                    return 'A';
                case 4:
                case 5:
                    return 'C';
                case 6:
                case 7:
                    return 'D';
                default:
                    return 'E';
            }
        }
    }
}