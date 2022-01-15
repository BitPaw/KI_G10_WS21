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

    public void Prepare()
    {
        // TestPrepareStuff(); // Debugging purposes: Comment everything within function function when used

        CreateDistributedGhosts();
    }


    public void Execute()
    {
        if (!cs.robotReady) return;

        // TestExecuteStuff(); // Debugging purposes: Comment everything within the function when used, except lines with robotReady

        List<int> ghostWeights = EvaluateGhostDistances();
        List<Tuple<int, int>> bestGhosts = SelectBestWeights(ghostWeights);
        ChangeGhostLocations(bestGhosts);
        MoveObjects();

        cs.robotReady = false;
    }

    private void ChangeGhostLocations(List<Tuple<int, int>> bestWeights)
    {
        // throw new NotImplementedException();
    }

    private List<int> EvaluateGhostDistances()
    {
        float robotDistance = MeasureRobotDistance();
        List<float> ghostDistances = MeasureGhostDistances();

        List<float> differences = CalculateDifferences(robotDistance, ghostDistances);

        List<int> probabilities = Normalize(differences);

        return probabilities;
    }

    private List<float> CalculateDifferences(float robot, List<float> ghosts)
    {
        return ghosts
            .Select(ghostDistance => Math.Abs(robot - ghostDistance - 1))
            .ToList();
    }

    private List<int> Normalize(List<float> values)
    {
        // min should always be 0
        float min = 0f;
        // max describes the tolerance between the distances of robot and ghost. 
        // If the difference exceeds tolerance, then the current probability is set to 0.
        float max = 10f;

        List<int> probabilities = values
            .Select(v => v <= max ? 100 - (int) ((v - min) / (max - min) * 100) : 0)
            .ToList();

        return probabilities;
    }

    private float MeasureRobotDistance()
    {
        return cs.robot.Scan();
    }

    private List<float> MeasureGhostDistances()
    {
        float ceiling = 100;

        return cs.ghosts
            .Select(g => g.GetDistance() < ceiling ? g.GetDistance() : ceiling)
            .ToList();
    }

    private List<Tuple<int, int>> SelectBestWeights(List<int> weights)
    {
        Random random = new Random();
        List<Tuple<int, int>> bestWeights = new List<Tuple<int, int>>();
        int chance;
        int index = 0;

        foreach (var weight in weights)
        {
            chance = random.Next(1, 101);
            if (chance <= weight)
                bestWeights.Add(Tuple.Create(index, weight));
            index++;
        }

        return bestWeights;
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
        // into unitys coordinate system.
        // Examples:
        //      (0, 0) top left of the map
        //      (106, 48) center of the map
        //      (212, 96) bottom right of the map
        float x = 106 - oldX;
        float y = 48 - oldY;
        return Tuple.Create(x, y);
    }

    #region Debugging functions

    private void TestPrepareStuff()
    {
        CreateGhostsForEvaluationTest();
    }

    private void TestExecuteStuff()
    {
        List<int> ghostWeights = EvaluateGhostDistances();
        List<Tuple<int, int>> bestGhosts = SelectBestWeights(ghostWeights);
        ChangeGhostLocations(bestGhosts);
        HighlightGhosts(bestGhosts);
        // DoNothing();
        MoveObjects();
    }

    private void DoNothing()
    {
        // it's required for proper debugging
        cs.robot.Move(0);
    }

    private void HighlightGhosts(List<Tuple<int, int>> chosenGhosts)
    {
        cs.ghosts.ForEach(g => g.ChangeColor(Color.red));
        chosenGhosts.ForEach(x => cs.ghosts[x.Item1].ChangeColor(Color.blue));
    }

    public void CreateGhostsForEvaluationTest()
    {
        CreateSquareGhosts(82f, 45f, 96.247f);
        CreateSquareGhosts(98f, 45f, 120);
        CreateSquareGhosts(110f, 40f, 90f);
        CreateSquareGhosts(0f, 40f, 0f);
        CreateSquareGhosts(207f, 40f, 180f);
        CreateSquareGhosts(106, 15f, 170f);
    }

    private void CreateSquareGhosts(float xCoord, float yCoord, float rotation)
    {
        for (int y = 0; y < 5; y++)
        for (float x = 0; x < 5; x++)
            CreateGhost(xCoord + x, yCoord + y, rotation);
    }

    #endregion
}