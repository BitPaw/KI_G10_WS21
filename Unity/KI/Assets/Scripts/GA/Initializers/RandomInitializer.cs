using System;
using System.Collections;
using System.Collections.Generic;

public class RandomInitializer : IInitializer
{
    private List<char> genes;
    private Random rand;
    public void AssignGene(char ID)
    {
        if (genes == null)
        {
            genes = new List<char>();
        }
        genes.Add(ID);
    }

    public List<Individual> CreateInitialGeneration(int generationSize, int individualSize)
    {
        List<Individual> list = new List<Individual>();
        for(int i = 0; i < generationSize; i++)
        {
            list.Add(GenerateIndividual(individualSize));
        }
        return list;
    }

    private Individual GenerateIndividual(int individualSize)
    {
        if (rand == null)
            rand = new Random();
        Individual ind = new Individual();
        System.Text.StringBuilder builder = new System.Text.StringBuilder(individualSize);
        int max = genes.Count;
        for(int i = 0; i < individualSize; i++)
        {
            builder.Append(genes[rand.Next(0,max)]);
        }
        ind.GeneSequence= builder.ToString();
        return ind;
    }
}
