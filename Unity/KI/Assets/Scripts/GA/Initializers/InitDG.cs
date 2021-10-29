using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;


public class InitDG : IInitializer
{
    private List<char> genes;
    private Random randomizer = new Random();

    public void AssignGene(char ID)
    {
        genes ??= new List<char>();
        genes.Add(ID);
    }

    public List<Individual> CreateInitialGeneration(int generationSize, int individualSize)
    {
        List<Individual> list = new List<Individual>();


        for (int i = 0; i < generationSize; i++)
        {
            Individual individual = new Individual();

            for (int j = 0; j < individualSize -1; j++)
            {
                var deployBreaks = j > individualSize - (individualSize / 5);

                if (deployBreaks)
                    individual.geneSequence += "E";
                else
                    individual.geneSequence += GetRandomGene();
            }

            individual.geneSequence += "F";

            Debug.Log(individual.geneSequence);
            list.Add(individual);
        }

        return list;
    }

    public string GetRandomGene()
    {

        switch (randomizer.Next(9))
        {
            case 0:
            case 1:
            case 2:
                return "B";
            case 3:
                return "A";
            case 4:
            case 5:
                return "C";
            case 6:
            case 7:
                return "D";
            case 8:
                return "E";
        }

        throw new Exception("Shouldn't occur");
    }


}