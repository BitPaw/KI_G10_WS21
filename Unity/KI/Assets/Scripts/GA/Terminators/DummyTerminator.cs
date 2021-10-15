using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTerminator : ITerminator
{
    private float chucksWrath = .2f;

    public bool JudgementDay(GenerationDB.Generation generation)
    {
        if (UnityEngine.Random.value < chucksWrath)
        {
            Debug.Log("The end is now.");
            return true;
        }
        else
        {
            Debug.Log("The end is nigh.");
            return false;
        }
    }
}
