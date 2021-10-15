using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneA : IGene {

    private CarController controller;
    public char ID
    {
        get
        {
            return 'A';
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
        controller.ApplyMotorTorque(1);
    }
}
