using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneE : IGene
{

    private CarController controller;
    public char ID
    {
        get
        {
            return 'E';
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
        controller.ApplyBrakes();
    }
}
