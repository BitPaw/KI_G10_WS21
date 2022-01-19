using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticleFilterBitPaw
{
    private ControllScript cs;

    public void ControllScriptSet(ControllScript controllScript)
    {
        cs = controllScript;
    }

    public void Prepare()
    {

        CreateDistributedGhosts();
    }
    public void Execute()
    {
        if (!cs.robotReady) return;

        //---<Get distance to wall values>-------------------------------------
        float wallDistanceRobot = cs.robot.Scan();
        List<float> wallDistanceGhost = cs.ghosts.Select(g => g.GetDistance()).ToList();
        //---------------------------------------------------------------------

        //---<Messure distances to wall>---------------------------------------
        for (int i = 0; i < wallDistanceGhost.Count; ++i)
        {
            float distanceToWall = Mathf.Abs(wallDistanceGhost[i]);
            float limit = 1f;
            bool isToNear = distanceToWall < limit;

            if (isToNear)
            {
                wallDistanceGhost[i] = float.PositiveInfinity;
            }
        }
        //---------------------------------------------------------------------


        //---<Rate values>-----------------------------------------------------       
        for (int i = 0; i < wallDistanceGhost.Count; ++i)
        {
            float difference = wallDistanceGhost[i] - wallDistanceRobot;

            wallDistanceGhost[i] = Mathf.Abs(difference); // How near is ghostwall to robowall
        }
        //---------------------------------------------------------------------

        //---<Get worst value>-------------------------------------------------
        float worst = 0;

        for (int i = 0; i < wallDistanceGhost.Count; ++i)
        {
            float value = wallDistanceGhost[i];

            if (worst < value && !float.IsInfinity(value))
            {
                worst = value;
            }
        }
        //---------------------------------------------------------------------

        //---<length to 0 to 1 -> rating 0=bad 1=perfect>----------------------
        for (int i = 0; i < wallDistanceGhost.Count; ++i)
        {
            float value = wallDistanceGhost[i];
            bool isValud = !float.IsInfinity(value);

            if (isValud)
            {
                wallDistanceGhost[i] = 1 - (wallDistanceGhost[i] / worst); // How near is gostwall to robowall (relativ)
            }
            else
            {
                wallDistanceGhost[i] = 0;
            }
        }
        //---------------------------------------------------------------------

        //---<Move bad Ghosts>-------------------------------------------------
        {
            Queue<GhostController> shittyGhosts = new Queue<GhostController>();
            List<GhostController> goodGhosts = new List<GhostController>();

            for (int i = 0; i < wallDistanceGhost.Count; ++i)
            {
                GhostController ghostController = cs.ghosts[i];
                float currentScore = wallDistanceGhost[i];
                float upperFilter = 0.975f;
                float lowerFilter = 0.65f;
                bool goodScore = currentScore >= upperFilter;
                bool scoreToBad = currentScore < lowerFilter;

                if (goodScore)
                {
                    goodGhosts.Add(ghostController);
                }

                if (scoreToBad)
                {
                    shittyGhosts.Enqueue(ghostController);
                }
            }

            while (shittyGhosts.Count > 0 && goodGhosts.Count > 0)
            {
                GhostController badScoreThing = shittyGhosts.Dequeue();
                int randomIndex = Random.Range(0, goodGhosts.Count-1);
                GhostController randomGoodPos = goodGhosts[randomIndex];

                SpawnAroundGhost(badScoreThing, randomGoodPos, 0.2f);
            }
        }
        //---------------------------------------------------------------------

        MoveObjects();

        cs.robotReady = false;
    }

    private void MoveObjects()
    {
        float distanceToWall = cs.robot.Scan();
        bool shouldTurn = distanceToWall < 2f;

        if (shouldTurn)
        {
            float degrees = 45f;
            cs.robot.Rotate(degrees);
            cs.ghosts.ForEach(g => g.Rotate(degrees));
        }
        else
        {
            float distance = 0.4f;
            cs.robot.Move(distance, cs.robot.MaxForward);
            cs.ghosts.ForEach(g => g.Move(distance));
        }
    }

    private void CreateDistributedGhosts()
    {
        const float SPACE = 0.2f;
        const float scalingX = 10;
        const float scalingY = 22;

        for (float y = -5; y < 5; y += SPACE)
        {
            for (float x = -5; x < 5; x += SPACE)
            {
                float degrees = Random.Range(0, 360);
                CreateGhost(x * scalingX, y * scalingY, degrees);
            }
        }
    }

    private void SpawnAroundGhost(GhostController sourceGhost, GhostController destGhost, float radius)
    {
        float randomXRadiusPos = Random.Range(-radius, radius);
        float randomYRadiusPos = Random.Range(-radius, radius);
        float randomRotation = Random.Range(0, 360);

        Vector3 targetPosCenter = destGhost.GetPosition();
        Vector3 offset = new Vector3(randomXRadiusPos, 0, randomYRadiusPos);
        Vector3 rotation = new Vector3(0, randomRotation, 0);

        sourceGhost.transform.eulerAngles = rotation;
        sourceGhost.transform.position = targetPosCenter + offset;
    }
    
    private void CreateGhost(float x, float y, float rotation)
    {
        Vector3 position = new Vector3(x, 0.57f, y);
        Quaternion quaternion = Quaternion.Euler(0f, rotation, 0f);

       cs.SpawnRobot(cs.Ghost, position, quaternion);
    }
}