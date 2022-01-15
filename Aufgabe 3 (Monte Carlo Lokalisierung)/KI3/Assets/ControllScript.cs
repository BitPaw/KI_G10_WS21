using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllScript : MonoBehaviour
{
    public List<GhostController> ghosts;
    public RobotController robot;
    public bool robotReady = false;
    public Transform Ghostspawner;
    private ParticleFilterDonst particleFilterDonst;

    public ControllScript()
    {
        particleFilterDonst = new ParticleFilterDonst(this);
    }

    void Start()
    {
        DeRegisterGhost(ghosts[0]);
        particleFilterDonst.Prepare();
    }

    void Update()
    {
        particleFilterDonst.Execute();
    }

    private void CreateGhost(Transform trans)
    {
        Instantiate(Ghost, trans.position, trans.rotation);
    }


    #region Stuff You Shouldn't Touch

    private static ControllScript self;
    public GameObject Ghost;

    void Awake()
    {
        if (self)
            Destroy(this);
        else
            self = this;
        ghosts = new List<GhostController>();
    }

    void OnDestroy()
    {
        self = null;
    }

    public static ControllScript GetInstance()
    {
        return self;
    }

    public void RegisterRobot(RobotController robot)
    {
        this.robot = robot;
    }

    public void RegisterGhost(GhostController ghost)
    {
        ghosts.Add(ghost);
    }

    public void DeRegisterGhost(GhostController ghost)
    {
        ghosts.Remove(ghost);
    }

    public void notifyRobotReady()
    {
        robotReady = true;
    }

    #endregion
}