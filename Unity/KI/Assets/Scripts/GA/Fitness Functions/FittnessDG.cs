using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FittnessDG : IFitnessFunction
{

    public float DetermineFitness(CarState state)
    {
        if (state.NumberOfCollisions() > 0)
            return 0f;

        float distance = 0.0f;
        float angle = 0.0f;
        float velocity = 0.0f;
        float tempVelocity = 0.0f;

        if (state.DistanceFromGoal() < 50)
            distance = 100 - state.DistanceFromGoal() * 2;

        if (state.AngleToGoal() < 4)
            angle = 100 - (state.AngleToGoal() * 100 / 4);


        if (state.CurrentVelocity() < 10)
        {
            velocity = 100 - Math.Abs(state.CurrentVelocity()) * 10;
        }

        return (distance * 6.0f + angle * 2.5f + velocity * 1.5f) / 1000;

    }
}
