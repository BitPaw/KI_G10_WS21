using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class ParticleFilterDonst : MonoBehaviour
{
    private ControllScript cs;

    private const int MAX_MAP_WIDTH = 106 * 2;
    private const int MAX_MAP_HEIGHT = 96 * 2;
    private int counter = 0;

    public ParticleFilterDonst(ControllScript controllScript)
    {
        cs = controllScript;
    }

    public void Execute()
    {
        if (!cs.robotReady) return;

        List<int> ghostWeights = EvaluateGhostDistances();

        Move();

        List<int> bestWeights = SelectBestWeights(ghostWeights);
        ChangeGhostLocations(bestWeights);

        cs.robotReady = false;
    }

    private void ChangeGhostLocations(List<int> bestWeights)
    {
        // throw new NotImplementedException();
    }

    private List<int> EvaluateGhostDistances()
    {
        float robotDistance = cs.robot.Scan();
        List<float> ghostDistances = cs.ghosts
            .Select(g => g.GetDistance() < 200 ? g.GetDistance() : 200)
            .ToList();

        List<float> weights = ghostDistances
            .Select(ghostDistance => Math.Abs(1 / (robotDistance - ghostDistance)))
            .ToList();
        float min = weights.Min(w => w);
        float max = weights.Max(w => w);

        List<int> normalizedWeights = weights.Select(value =>
            (int) ((value - min) / (max - min) * 100)).ToList();
        return normalizedWeights;
    }

    private List<int> SelectBestWeights(List<int> weights)
    {
        Random random = new Random();
        List<int> indexes = new List<int>();
        int index = 0;

        foreach (var weight in weights)
        {
            if (weight <= random.Next(0, 101))
                indexes.Add(index);
            index++;
        }

        return indexes;
    }

    private void Move()
    {
        if (cs.robot.Scan() < 2f)
        {
            float degrees = counter++ % 2 == 0 ? 120f : -120f;
            cs.robot.Rotate(degrees);
            cs.ghosts.ForEach(g => g.Rotate(degrees));
        }
        else
        {
            float distance = 0.1f;
            cs.robot.Move(distance, 100);
            cs.ghosts.ForEach(g => g.Move(distance));
        }
    }

    public void CreateDistributedGhosts()
    {
        Random random = new Random();
        const int SPACE = 2;
        float degrees;

        for (int y = 0; y <= MAX_MAP_HEIGHT; y += SPACE)
        {
            for (int x = 0; x <= MAX_MAP_WIDTH; x += SPACE)
            {
                degrees = random.Next(0, 361);
                CreateGhost(x, y, degrees);
            }
        }
    }


    public void CreateGhost(float x, float y, float rotation)
    {
        (float newX, float newY) = TranslateCoordinates(x, y);
        cs.Ghostspawner.position = new Vector3(newY, 0.57f, newX);
        cs.Ghostspawner.rotation = Quaternion.Euler(0f, rotation, 0f);
        Instantiate(cs.Ghost, cs.Ghostspawner.position, cs.Ghostspawner.rotation);
    }

    private Tuple<float, float> TranslateCoordinates(float oldX, float oldY)
    {
        // This function maps a custom coordinate system
        // into the unity coordinate system.
        // Examples:
        //      (0, 0) top left of the map
        //      (106, 48) center of the map
        //      (212, 96) bottom right of the map
        float x = 106 - oldX;
        float y = 48 - oldY;
        return Tuple.Create(x, y);
    }
    
}