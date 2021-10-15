using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mutatotron = UnityEngine.Random;

public class DummyMutator : IMutator
{
    static string[] abilities =
    {
        "fly","regenerate 2d6 health once per day", "read the minds of molluscs", "move at the speed of an old lady at the checkout counter",
        "die horribly from cancer", "speak with grass", "sense situationally inappropriate erections within 60ft", "do ninja stuff", "shoot lasers out of its eyes (1mW)",
        "explain the function of a Plumbus", "change the fabric of reality at will", "bring peace to the middle east", "prove their innocence in the Lindbergh kidnapping case"
    };

    private float mutationRate = .1f;
    private List<char> geneIDs;

    public void AssignGene(char ID)
    {
        if (geneIDs == null)
            geneIDs = new List<char>();
        geneIDs.Add(ID);
    }

    public string Mutate(string original)
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        for(int i=0; i< original.Length; i++)
        {
            if (Mutatotron.value < mutationRate)
                builder.Append(geneIDs[Mutatotron.Range(0, geneIDs.Count)]);
            else
                builder.Append(original[i]);
        }
//        Debug.Log("MutatOTron blasted "+original+" into "+builder.ToString()+ ", it can now " + GetResult());
        return builder.ToString();
    }

    private string GetResult()
    {
        return abilities[Mutatotron.Range(0, abilities.Length)];
    }
}
