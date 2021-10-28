using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminatorDG : ITerminator
{

    public bool JudgementDay(GenerationDB.Generation generation)
    {
        return generation.Fittest.Fitness >= 0.98f;
    }
}