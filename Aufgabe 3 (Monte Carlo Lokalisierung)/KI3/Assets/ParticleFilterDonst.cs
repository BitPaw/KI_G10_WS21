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

        List<int> moveGhost = new List<int>();
        var bestweightCount = 0;


        for (int i = 0; i < cs.ghosts.Count-1; i++)
        {
            while (bestWeights[bestweightCount].Item1 < i && bestweightCount <= bestweightCount -1)
            {
                bestweightCount++;
            }

            if(bestWeights[bestweightCount].Item1 != i)
            {
                moveGhost.Add(i);
            }
        }

        //for (int i = cs.ghosts.Count - 1; i >= 0; i--)
        //{
        //    var isInBest = bestWeights.Any(bw => bw.Item1 == i);

        //    if (!isInBest)
        //        moveGhost.Add(i);
        //}

        Random random = new Random();

        foreach (var ghostID in moveGhost)
        {
            var ghost = cs.ghosts[ghostID];

            var destGhost = cs.ghosts[bestWeights[random.Next(0, bestWeights.Count)].Item1];


            SpawnAroundGhost(ghost, destGhost, 1.5f);
        }

        Debug.Log(cs.ghosts.Count);
    }


    private List<int> DistributeRemainingGhostAmount(List<float> weights, int amount)
    {
        var totalWeight = weights.Sum();
        var length = weights.Count();

        var actual = new double[length];
        var error = new double[length];
        var rounded = new int[length];

        var added = 0;

        var i = 0;
        foreach (var w in weights)
        {
            actual[i] = amount * (w / totalWeight);
            rounded[i] = (int) Math.Floor(actual[i]);
            error[i] = actual[i] - rounded[i];
            added += rounded[i];
            i += 1;
        }

        while (added < amount)
        {
            var maxError = 0.0;
            var maxErrorIndex = -1;
            for (var e = 0; e < length; ++e)
            {
                if (error[e] > maxError)
                {
                    maxError = error[e];
                    maxErrorIndex = e;
                }
            }

            rounded[maxErrorIndex] += 1;
            error[maxErrorIndex] -= 1;

            added += 1;
        }

        return rounded.ToList();
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


    private void SpawnAroundGhost(GhostController sourceGhost, GhostController destGhost, float radius)
    {
        // Spawns a existing ghost (sourceGhost) around a ghost (destGhost)
        Random random = new Random();

        float destAngle = destGhost.GetRotation().y;
        (float destX, float destY) =
            TranslateCoordinates(destGhost.transform.position.z, destGhost.transform.position.x);

        float randomRadius = radius * (float) Math.Sqrt(random.NextDouble());
        float randomTheta = (float) (random.NextDouble() * 2 * Math.PI);

        float xDeviation = randomRadius * (float) Math.Cos(randomTheta);
        float yDeviation = randomRadius * (float) Math.Sin(randomTheta);
        float angleDeviation = -1 + (float) (random.NextDouble() * 2); // Deviation between -1 and 1

        float sourceAngle = destAngle + angleDeviation;
        (float sourceX, float sourceY) =
            TranslateCoordinates(destX + xDeviation, destY + yDeviation);

        sourceGhost.transform.eulerAngles = new Vector3(0, sourceAngle, 0);
        sourceGhost.transform.position = new Vector3(sourceY, 0.57f, sourceX);
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
        CreateGhost(82f, 45f, 96.247f);
        CreateGhost(82f, 45f, 96.247f);
        // CreateDistributedGhosts();
        // CreateSquareGhosts(82f, 45f, 96.247f);
        // CreateGhostsForEvaluationTest();
    }

    private void TestExecuteStuff()
    {
        // List<int> ghostWeights = EvaluateGhostDistances();
        // List<Tuple<int, int>> bestGhosts = SelectBestWeights(ghostWeights);
        // ChangeGhostLocations(bestGhosts);
        // HighlightGhosts(bestGhosts);
        SpawnAroundGhost(cs.ghosts[1], cs.ghosts[0], 0.5f);
        DoNothing();
        // MoveObjects();
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

    private void SpawnAroundGhost(GhostController ghost, float radius)
    {
        // Spawns a new ghost around a ghost
        Random random = new Random();
        float angle = ghost.transform.eulerAngles.y;
        (float x, float y) =
            TranslateCoordinates(ghost.transform.position.z, ghost.transform.position.x);

        float randomRadius = radius * (float) Math.Sqrt(random.NextDouble());
        float randomTheta = (float) (random.NextDouble() * 2 * Math.PI);

        float xDeviation = randomRadius * (float) Math.Cos(randomTheta);
        float yDeviation = randomRadius * (float) Math.Sin(randomTheta);
        float angleDeviation = -1 + (float) (random.NextDouble() * 2); // Deviation between -1 and 1

        CreateGhost(x + xDeviation, y + yDeviation, angle + angleDeviation);
    }

    #endregion
}