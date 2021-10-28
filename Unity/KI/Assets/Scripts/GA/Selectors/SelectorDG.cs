using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//using UnityEngine;


public class SelectorDG : ISelector
{

    public List<string> SelectFromGeneration(GenerationDB.Generation parentGeneration)
    {
        Debug.Log("{AAA:" + parentGeneration.individuals.Count);
        parentGeneration.Sort();
        List<string> selectedGenes = new List<string>(parentGeneration.individuals.Count * 2);

        for (int i = 0; i < parentGeneration.individuals.Count /2; i++)
        {
            selectedGenes.Add(parentGeneration.individuals[i].geneSequence);
            selectedGenes.Add(parentGeneration.individuals[i+1].geneSequence);
        }
        for (int i = 0; i < parentGeneration.individuals.Count / 2; i++)
        {
            selectedGenes.Add(parentGeneration.individuals[i].geneSequence);
            selectedGenes.Add(parentGeneration.individuals[i + 2].geneSequence);
        }

        Debug.Log("AAA}:" + selectedGenes.Count());

        return selectedGenes;

    }

}