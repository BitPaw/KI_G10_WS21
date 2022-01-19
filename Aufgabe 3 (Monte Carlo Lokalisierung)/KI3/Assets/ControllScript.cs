using System.Collections.Generic;
using UnityEngine;

public class ControllScript : MonoBehaviour
{
    [SerializeField] public List<GhostController> ghosts;
    [SerializeField] public RobotController robot;
    [SerializeField] public bool robotReady = false;
    [SerializeField] public Transform Ghostspawner;
    [SerializeField] private ParticleFilterBitPaw particleFilterDonst;

    public ControllScript()
    {
        particleFilterDonst = new ParticleFilterBitPaw();
        particleFilterDonst.ControllScriptSet(this);
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

    public void SpawnRobot(GameObject ghost, Vector3 position, Quaternion rotation)
    {
        Instantiate(ghost, position, rotation);
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