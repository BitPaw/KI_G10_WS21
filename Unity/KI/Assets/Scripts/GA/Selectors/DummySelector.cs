using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySelector : ISelector
{
    public List<string> SelectFromGeneration(GenerationDB.Generation parentGeneration)
    {
        List<string> superAwesomeNewGen = new List<string>();
        foreach (Individual ind in parentGeneration.Individuals)
        {
            superAwesomeNewGen.Add(ind.GeneSequence);
            superAwesomeNewGen.Add(ind.GeneSequence);
        }
        return superAwesomeNewGen;
    }
}
