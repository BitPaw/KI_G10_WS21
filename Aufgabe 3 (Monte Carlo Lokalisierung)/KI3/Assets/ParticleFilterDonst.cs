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
        List<int> bestGhosts = SelectBestWeights(ghostWeights);
        ChangeGhostLocations(bestGhosts);
        HighlightGhosts(bestGhosts); // OPTIONAL: Debugging purposes
        MoveObjects();

        cs.robotReady = false;
    }

    private void ChangeGhostLocations(List<int> bestWeights)
    {
        // throw new NotImplementedException();
    }

    private List<int> EvaluateGhostDistances()
    {
        float robotDistance = MeasureRobotDistance();
        List<float> ghostDistances = MeasureGhostDistances();

        List<float> weights = CalculateDifferences(robotDistance, ghostDistances);
        weights = NormalizeValues(weights);

        List<int> probabilities = ToProbabilities(weights);

        return probabilities;
    }

    private List<float> CalculateDifferences(float robot, List<float> ghosts)
    {
        return ghosts
            .Select(ghostDistance => Math.Abs(robot - ghostDistance))
            .ToList();
    }

    private List<float> NormalizeValues(List<float> values)
    {
        float sum = values.Sum();

        return values.Select(w => w / sum).ToList();
    }

    private List<int> ToProbabilities(List<float> values)
    {
        float min = values.Min();
        float max = values.Max();

        return values
            .Select(value => (value - min) / (max - min))
            .Select(prob => prob >= 0.99f ? 99 : (int) (prob * 100))
            .ToList();
    }

    private float MeasureRobotDistance()
    {
        return cs.robot.Scan();
    }

    private List<float> MeasureGhostDistances()
    {
        return cs.ghosts
            .Select(g => g.GetDistance() < 200 ? g.GetDistance() : 200)
            .ToList();
    }

    private List<int> SelectBestWeights(List<int> weights)
    {
        Random random = new Random();
        List<int> indexes = new List<int>();
        int chance;
        int index = 0;

        foreach (var weight in weights)
        {
            chance = random.Next(1, 101);
            if (chance <= weight)
                indexes.Add(index);
            index++;
        }

        return indexes;
    }

    private void MoveObjects()
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

    private void HighlightGhosts(List<int> chosenGhosts)
    {
        // For test purposes
        cs.ghosts.ForEach(g => g.ChangeColor(Color.red));
        chosenGhosts.ForEach(i => cs.ghosts[i].ChangeColor(Color.blue));
    }
}