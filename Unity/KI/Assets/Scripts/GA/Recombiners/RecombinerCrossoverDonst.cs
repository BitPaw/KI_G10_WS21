using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RecombinerCrossoverDonst : IRecombiner
{
    public string Combine(string parentA, string parentB)
    {
        Random random = new Random();
        int length = parentA.Length;
        int position = random.Next(0, length);

        return parentA.Substring(0, position) +
               parentB.Substring(position, length - position);
    }
}