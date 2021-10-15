using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneD : IGene
{

    private CarController controller;
    public char ID
    {
        get
        {
            return 'D';
        }
    }

    public CarController Controller
    {
        get
        {
            return controller;
        }
        set
        {
            controller = value;
        }
    }
    public void Execute()
    {
        controller.ApplySteering(-1);
    }
}
