using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTerm : ITerminator
{
    public bool JudgementDay(GenerationDB.Generation generation)
    {
        return true;
//        throw new NotImplementedException();
    }
}
