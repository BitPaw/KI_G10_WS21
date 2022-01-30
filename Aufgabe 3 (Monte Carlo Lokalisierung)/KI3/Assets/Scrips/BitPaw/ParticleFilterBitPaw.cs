using BitPaw;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFilterBitPaw
{
    private ControllScript cs = null;

    // Settings
    bool HandleGhostWallCollisionAsOutOfBounce = true;
    bool FilterTooFarAway = false;
    bool FilerOutOfBounce = true;
    float FilterValueMinimalValue = 1;
    float FilterValueRatingGood = 0.005f;
    float FilterValueRatingBad = 2.0f;
    float GhostsSpawnSpacing = 0.08f; // bigger=>fewer, smaler=>more


    int AmountOfRays = 5;
    float CutOfRange = 5; // Image Rage of Units
    float TotalScore = 0;
    float RepositionRadius = 0.5f;

    bool EnableRayCastDirectionDraw = true;

    private SensorDataHistory _sensorDataHistory = new SensorDataHistory(); // GhostData
    private SensorDataVieldOfView _sensorDataVieldOfViewRobot = new SensorDataVieldOfView(); // RobotView

    public void ControllScriptSet(ControllScript controllScript)
    {
        cs = controllScript;
    }

    public void Prepare()
    {
        CreateDistributedGhosts();
    }

    private void InputDataUpdate()
    {
        int amountofValues = cs.ghosts.Count;
       
        SensorDataSet sensorDataSet = _sensorDataHistory.GetNext(); // Get current data
        sensorDataSet.Reserve(amountofValues); // Allocate memory if needed

        for (int i = 0; i < amountofValues; i++) // Robot loop
        {
            GhostController ghostController = cs.ghosts[i];
            SensorDataVieldOfView sensorData = sensorDataSet.SensorDataVieldOfViewList[i];            

            sensorData.Reserve(AmountOfRays); // Allocate memory if needed

            QuickMaths.SpreadedRayCast(ghostController.transform, AmountOfRays, ref sensorData.DistanceToWall, true);        
        }        
    }

    private float RateFromData(float[] target, float[] data, int dataSize)
    {
        float rating = 0;

        for (int v = 0; v < dataSize; v++)
        {
            float value = data[v];
            bool impossibleValue = value == float.PositiveInfinity;

            if (impossibleValue)
            {
                rating = 0; // impossible values are valed as 0
                break;
            }
            else
            {
                float distanceToWall = data[v] - target[v]; 
                float distanceToWallAbsolute = Mathf.Abs(distanceToWall);

                if (distanceToWallAbsolute > CutOfRange)
                {
                    rating = 0;
                }
                else
                {
                    rating += distanceToWallAbsolute;
                }           
            }
        }

        rating /= (float)dataSize;

        return rating;
    }

    private void InputDataRate()
    {
        float inputRobotDistance = cs.robot.Scan();
        SensorDataSet sensorDataSet = _sensorDataHistory.GetCurrent();

        _sensorDataVieldOfViewRobot.Reserve(AmountOfRays);
       
        QuickMaths.SpreadedRayCast(cs.robot.transform, AmountOfRays, ref _sensorDataVieldOfViewRobot.DistanceToWall, true);

        for (int i = 0; i < sensorDataSet.SensorDataVieldOfViewListSize; i++)
        {
            SensorDataVieldOfView sensorDataVieldOfView = sensorDataSet.SensorDataVieldOfViewList[i];

            sensorDataVieldOfView.Rating = RateFromData(_sensorDataVieldOfViewRobot.DistanceToWall, sensorDataVieldOfView.DistanceToWall, AmountOfRays);

            cs.ghosts[i].Rating = sensorDataVieldOfView.Rating;
        }
    }

    private void InputDataColor()
    {
        SensorDataSet sensorDataSet = _sensorDataHistory.GetCurrent();

        for (int i = 0; i < sensorDataSet.SensorDataVieldOfViewListSize; i++)
        {
            SensorDataVieldOfView sensorDataVieldOfView = sensorDataSet.SensorDataVieldOfViewList[i];
            float rating = sensorDataVieldOfView.Rating;
            Color color;

            if (rating == float.PositiveInfinity)
            {
                // Distance to far
                color = new Color(0, 0, 1);
            }
            else
            {
                if (float.IsNaN(rating) && float.IsNaN(float.NaN))
                {
                    color = new Color(1, 0, 1);
                }
                else
                {
                    color = new Color(1-rating, rating, 0);
                }
            }

            cs.ghosts[i].ChangeColor(color);
        }        
    }

    public void Execute()
    {     
        InputDataUpdate();
        InputDataRate();
        InputDataColor();

        if (true)
        {
            GhostReposition();
        }

        MoveObjects();  

        // cs.robotReady = false;
    }

    private void GhostReposition()
    {
        List<int> spawnCandiates = new List<int>();
        List<int> reposCandiates = new List<int>();
        SensorDataSet sensorDataSet = _sensorDataHistory.GetCurrent();

        TotalScore = 0;

        for (int i = 0; i < sensorDataSet.SensorDataVieldOfViewListSize; i++)
        {
            SensorDataVieldOfView sensorDataVieldOfView = sensorDataSet.SensorDataVieldOfViewList[i];
            float rating = sensorDataVieldOfView.Rating;
            bool reposition = rating == 0;
            bool goodScore = rating >= 1f;

            TotalScore += rating;

            if (reposition)
            {
                reposCandiates.Add(i);
            }

            if (goodScore)
            {
                spawnCandiates.Add(i);          
            }
        }   

        int spawnRange = spawnCandiates.Count;

        Debug.Log("Score : " + TotalScore);

        if (TotalScore >= 0.80f)
        {
            Debug.Log("Finsihed");
           // Debug.Break();
        }     

        if (spawnRange > 0)
        {
            foreach (int index in reposCandiates)
            {
                int spawnIndex = Random.Range(0, spawnRange - 1);
                int spawnID = spawnCandiates[spawnIndex];

                GhostController ghostNonHit = cs.ghosts[index];
                GhostController spawnCadidatPos = cs.ghosts[spawnID];

                SpawnAroundGhost(ghostNonHit, spawnCadidatPos, RepositionRadius);
            }
        }

        spawnCandiates.Clear();
        reposCandiates.Clear();
    }

    private void MoveObjects()
    {
        float distanceToWall = cs.robot.Scan();
        bool shouldTurn = distanceToWall < 3f;

        Transform transform = cs.robot.transform;

        if (shouldTurn)
        {
            float degrees = 90f * Time.deltaTime;
   
            Vector3 oldscaling = transform.localScale;
            transform.localScale = new Vector3(1, 1, 1);
            //transform.Rotate(0, degrees, 0);
            cs.robot.Rotate(degrees);
            transform.localScale = oldscaling;

            cs.ghosts.ForEach(g => g.Rotate(degrees));
        }
        else
        {
            float distance = 20f * Time.deltaTime;
            Vector3 oldscaling = transform.localScale;
            transform.localScale = new Vector3(1, 1, 1);

            cs.robot.Move(distance, cs.robot.MaxForward);
            cs.ghosts.ForEach(g => g.Move(distance*0.13f));

            transform.localScale = oldscaling;
        }
    }

    private void CreateDistributedGhosts()
    {
        const float scalingX = 10;
        const float scalingY = 22;

        for (float y = -5; y < 5; y += GhostsSpawnSpacing)
        {
            for (float x = -5; x < 5; x += GhostsSpawnSpacing)
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

#region [Limbo-Garbage]
/*
 * 
 * 
 * 
        /*
               float standardDeviation = QuickMaths.StandardDeviationCalculate(_inputGhostDistance, _inputGhostDistanceSize, avrage);
      

        for (int i = 0; i < _inputGhostDistanceSize; i++)
        {
            float inputGhostDisctance = _inputGhostDistance[i];

            if (inputGhostDisctance == float.PositiveInfinity) // OutOfBounce
            {
                continue; // Skip to next check
            }

            if (inputGhostDisctance < FilterValueMinimalValue && HandleGhostWallCollisionAsOutOfBounce) // Robot can only messure above 1 (size of Robot)[]Min distance
            {
                _inputGhostDistance[i] = float.PositiveInfinity; // handle sameway as outofbounce
                continue;
            }

            float distrubution = QuickMaths.NormalDistribution(inputRobotDistance, inputGhostDisctance, standardDeviation);
            float calc = Mathf.Abs(distrubution);

            _inputGhostDistance[i] = calc; // Save      
        }* /






float wallDistanceRobot = cs.robot.Scan();
        int wallDistanceGhostListSize = cs.ghosts.Count;
        float[] wallDistanceGhostList = new float[wallDistanceGhostListSize];

        int howManySteps = 8;
        int interationStep = 360 / howManySteps;

        for (int i = 0; i < wallDistanceGhostListSize; i++)
        {
            wallDistanceGhostList[i] = float.PositiveInfinity;
        }

        for (int i = interationStep; i < interationStep+1; i += interationStep)
        {
            float rotation = interationStep;

            for (int j = 0; j < wallDistanceGhostListSize; j++)
            {
                GhostController ghostController = cs.ghosts[j];                

                ghostController.Rotate(rotation);

                float distance = ghostController.GetDistance();

                if (distance != float.PositiveInfinity)
                {
                    if (wallDistanceGhostList[j] == float.PositiveInfinity) // Pretent as invalid, just override
                    {
                        wallDistanceGhostList[j] = distance; // WRITE
                    }
                    else
                    {
                        bool isSmaler = distance < wallDistanceGhostList[j];

                        if (isSmaler)
                        {
                            wallDistanceGhostList[j] = distance; // WRITE
                        }
                    }                  
                }                     
            }
        }


        //if (!cs.robotReady) return;


        for (int i = 0; i < wallDistanceGhostListSize; i++)
        {
            float vlaue = Mathf.Abs(wallDistanceGhostList[i] - wallDistanceRobot);
            Color color;

            if (vlaue == float.PositiveInfinity)
            {
                color = new Color(0,0,1);
            }
            else
            {
                if (vlaue < 30)
                {
                    color = new Color(0, 1, 0);
                }
                else
                {
                    color = new Color(1, 0, 0);
                }
            }

            cs.ghosts[i].ChangeColor(color); 
        }


        return;

        // 






        /*
         * TEST FOR DISTRIBUTED
         * 
        float[] values = new float[7] { 1,2,3,4,5,6,7 };

        float roboterInput = 4; // y
        float dev = CalcO(values, 7, roboterInput);// o

        for (int i = 0; i < 7; i++)
        {
           
            float ghostInput = i;

            float tww = Stand(ghostInput, roboterInput, dev);

            tww += 15;
        }* /

//float te = Probability(12,24,0,50);
//float ta = Probability(23, 24, 0, 50);
//loat tq = Probability(20, 24, 0, 50);
















//Settings
bool reposition = true;
        bool filterToFarWay = true;
        bool reposNonHit = true;
        bool furtherThenReal = true;


        //---<Get distance to wall values>-------------------------------------
        float wallDistanceRobot = cs.robot.Scan();
        List<float> wallDistanceGhost = cs.ghosts.Select(g => g.GetDistance()).ToList();
        //---------------------------------------------------------------------

        List<int> primarySpawnCandiates = new List<int>();

        // Repoition non-hiiting  -> OutOfBounce
        if (reposNonHit)        
        {        
            List<int> reposCandiates = new List<int>();

            for (int i = 0; i < wallDistanceGhost.Count; i++)
            {
                if (wallDistanceGhost[i] != float.PositiveInfinity)
                {
                    primarySpawnCandiates.Add(i);
                }
                else
                {
                    reposCandiates.Add(i);
                }
            }

            // repos
            int maxRange = primarySpawnCandiates.Count;

            foreach (int index in reposCandiates)
            {
                GhostController ghostNonHit = cs.ghosts[index];
                int spawnIndex = Random.Range(0, maxRange - 1);
                GhostController spawnCadidatPos = cs.ghosts[spawnIndex];

                SpawnAroundGhost(ghostNonHit, spawnCadidatPos, 2.0f);
            }
        }


        if (furtherThenReal)
        {
            List<int> secSpawnPos = new List<int>();
            List<int> reposCandiates = new List<int>();

            for (int i = 0; i < primarySpawnCandiates.Count; ++i)
            {
                int index = primarySpawnCandiates[i];
                float value = wallDistanceGhost[index];
                bool ismoreNear = Mathf.Abs(value - wallDistanceRobot) > 2;

                if (ismoreNear)
                {
                    reposCandiates.Add(i);
                }
                else
                {
                    secSpawnPos.Add(i);
                }
            }


            int maxRange = secSpawnPos.Count;

            foreach (int index in reposCandiates)
            {
                GhostController ghostNonHit = cs.ghosts[index];
                int spawnIndex = Random.Range(0, maxRange - 1);
                GhostController spawnCadidatPos = cs.ghosts[spawnIndex];

                SpawnAroundGhost(ghostNonHit, spawnCadidatPos, 0.2f);
            }
        }

        //---<Messure distances to wall>---------------------------------------
        if (filterToFarWay)
        {
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
        }        
        //---------------------------------------------------------------------


        //---<Rate values>-----------------------------------------------------       
        for (int i = 0; i < wallDistanceGhost.Count; ++i)
        {
            float difference = wallDistanceGhost[i] - wallDistanceRobot -1;

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
                float upperFilter = 0.995f;
                float lowerFilter = 0.20f;
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

            while (shittyGhosts.Count > 0 && goodGhosts.Count > 0 && reposition)
            {
                GhostController badScoreThing = shittyGhosts.Dequeue();
                int randomIndex = Random.Range(0, goodGhosts.Count - 1);
                GhostController randomGoodPos = goodGhosts[randomIndex];

                SpawnAroundGhost(badScoreThing, randomGoodPos, 0.2f);
            }
        }
        //---------------------------------------------------------------------   
   



 * */
#endregion