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

    private List<float> EvaluateGhostDistances()
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

        return weights.Select(value => (value - min) / (max - min)).ToList();
    }

    public void Execute()
    {
        if (!cs.robotReady) return;

        List<float> ghostWeights = EvaluateGhostDistances();

        Move();

        ColorBestGhosts(ghostWeights, 0.1f, Color.blue);

        cs.robotReady = false;
    }

    private void ColorBestGhosts(List<float> weights, float threshold, Color color)
    {
        List<int> indexes = new List<int>();
        int index = 0;

        foreach (var weight in weights)
        {
            if (weight >= threshold)
                indexes.Add(index);
            index++;
        }

        indexes.ForEach(i => cs.ghosts[i].ChangeColor(color));
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
        // Width -106 (right) to 106 (left)
        // Height -48 (bottom) to 48 (top)

        Random random = new Random();
        const int SPACE = 2;
        float degrees;

        for (int y = 0; y <= MAX_MAP_HEIGHT; y += SPACE)
        {
            for (int x = 0; x <= MAX_MAP_WIDTH; x += SPACE)
            {
                degrees = random.Next(0, 361);
                (float newX, float newY) = TranslateCoordinates(x, y);
                CreateGhost(newX, newY, degrees);
            }
        }
    }


    private void CreateGhost(float x, float y, float rotation)
    {
        cs.Ghostspawner.position = new Vector3(y, 0.57f, x);
        cs.Ghostspawner.rotation = Quaternion.Euler(0f, rotation, 0f);
        Instantiate(cs.Ghost, cs.Ghostspawner.position, cs.Ghostspawner.rotation);
    }

    private Tuple<float, float> TranslateCoordinates(float oldX, float oldY)
    {
        // Coordinate(0, 0) represents the middle of the map
        float x = 106 - oldX;
        float y = 48 - oldY;
        return Tuple.Create(x, y);
    }
}