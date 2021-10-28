using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminatorDonst : ITerminator
{
    public bool JudgementDay(GenerationDB.Generation generation)
    {
        return generation.Fittest.Fitness >= 0.9f;
    }
}
