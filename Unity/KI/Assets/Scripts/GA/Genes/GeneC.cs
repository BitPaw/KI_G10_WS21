using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneC : IGene
{

    private CarController controller;
    public char ID
    {
        get
        {
            return 'C';
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
        controller.ApplySteering(1);
    }
}
