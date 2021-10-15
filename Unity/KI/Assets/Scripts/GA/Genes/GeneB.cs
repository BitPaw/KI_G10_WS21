using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneB : IGene
{

    private CarController controller;
    public char ID
    {
        get
        {
            return 'B';
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
        controller.ApplyMotorTorque(-1);
    }
}
