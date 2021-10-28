using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MutatorDG : IMutator
{
    private List<char> geneIDs;

    private readonly Random randomizer = new Random();

    public void AssignGene(char ID)
    {
        geneIDs ??= new List<char>();
        geneIDs.Add(ID);
    }

    public string Mutate(string original)
    {
        var tempString = original;

        for (int i = 0; i < randomizer.Next(4); i++)
        {
            var mutatePosition = randomizer.Next(original.Length);

            tempString = tempString.Remove(mutatePosition, 1).Insert(mutatePosition, GetRandomGene());
        }

        return tempString;
    }



    public string GetRandomGene()
    {
        switch (randomizer.Next(5))
        {
            case 0:
                return "A";
            case 1:
                return "B";
            case 2:
                return "C";
            case 3:
                return "D";
            case 4:
                return "E";
        }

        throw new Exception("Shouldn't occur");
    }
}