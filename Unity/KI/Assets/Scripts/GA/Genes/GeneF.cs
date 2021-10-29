using System.Collections;
using UnityEngine;


public class GeneF : IGene
{

    private CarController controller;

    public char ID
    {
        get { return 'F'; }
    }

    public CarController Controller
    {
        get { return controller; }
        set { controller = value; }
    }

    public void Execute()
    {
        controller.ApplyMotorTorque(0);
    }
}
