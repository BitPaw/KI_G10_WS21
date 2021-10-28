using System;
using System.Collections.Generic;
using UnityEngine;

namespace GA.Fitness_Functions
{
    public class FittnessHexClaw : IFitnessFunction
    {
        Dictionary<int, Tuple<float,float,int>> stats = new Dictionary<int, Tuple<float, float, int>>();
        public float DetermineFitness(CarState state)
        {            
            const float distanceMultiplicator = 0.45f;
            const float angleMultiplicator = 0.55f;
            const float parkoutPunishment = 0.05f;

            float distanceRaw = 1 - Mathf.Abs(state.DistanceFromGoal());
            float angleRaw = 1 - Mathf.Abs(state.AngleToGoal());

            float distance = distanceRaw * distanceMultiplicator;
            float angle = angleRaw * angleMultiplicator;

            float collisions = 1 + state.NumberOfCollisions();

            if(!stats.ContainsKey(state.GetInstanceID()))
            {
                stats.Add(state.GetInstanceID(),new Tuple<float, float, int>(distance,angle,0));
            }
            else
            {
                float distanceUpdated = distance;
                float angleUpdated = angle;
                bool worsend = false;

                if(distance>stats[state.GetInstanceID()].Item1)
                {
                    distanceUpdated -= parkoutPunishment * (1 + stats[state.GetInstanceID()].Item3);
                    worsend = true;
                }

                if(worsend)
                {
                    int failsInARow = stats[state.GetInstanceID()].Item3+1;
                    stats[state.GetInstanceID()] = new Tuple<float, float, int>(distance,angle,failsInARow);
                }
                else
                {
                    stats[state.GetInstanceID()] = new Tuple<float, float, int>(distance, angle, 0);
                }

                distance = distanceUpdated;
                angle = angleUpdated;
            }

            if (distance + angle >= 0.9f)
            {
                collisions = ((collisions - 1) * 2) + 1;
            }

            return (((distance + angle) / collisions));
        }
    }
}
