using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyRecombiner : IRecombiner
{
    public string Combine(string parentA, string parentB)
    {
        return parentA;
    }
}
