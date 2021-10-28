using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

//using UnityEngine;

public class RecombinerDG : IRecombiner
{
    private readonly Random randomizer = new Random();

    public string Combine(string parentA, string parentB)
    {
        var tempString = parentA;

        for (int i = 0; i < randomizer.Next(4); i++)
        {
            var recombinePosition = randomizer.Next(parentA.Length);

            tempString = tempString.Remove(recombinePosition, 1).Insert(recombinePosition, parentB[recombinePosition].ToString());;
        }

        return tempString;
    }

}