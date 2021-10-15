using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores a sequence and its associated Fitness value.
/// Author: Sascha Schewe
/// </summary>
public class Individual{
    public string geneSequence;
    public float fitnessValue;

    /// <summary>
    /// Important: Can only be assigned once, changed/new sequences should be assigned to new Individuals.
    /// </summary>
    public string GeneSequence
    {
        get
        {
            return geneSequence;
        }

        set
        {
            if (geneSequence != null)
                throw new System.Exception("Individuals can only have a sequence assigned once");
            geneSequence = value;
        }
    }

    public float Fitness
    {
        set
        {
            fitnessValue = value;
        }
        get
        {
            return fitnessValue;
        }
    }
}
