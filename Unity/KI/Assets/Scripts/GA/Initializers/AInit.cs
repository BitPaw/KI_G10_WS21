using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AInit : IInitializer
{
    public void AssignGene(char ID)
    {
        return;
//        throw new NotImplementedException();
    }

    public List<Individual> CreateInitialGeneration(int generationSize, int individualSize)
    {
        Debug.Log("AInit");
        return new List<Individual>();
//        throw new NotImplementedException();
    }
}
