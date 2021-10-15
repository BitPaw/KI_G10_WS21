using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour {

    private bool paused = false;
    private float standardTimeScale = 0;

    public void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;
        if (!paused)
        {
            standardTimeScale = Time.timeScale;
            Time.timeScale = 0.00001f;
            paused = true;
        }
        else
        {
            Time.timeScale = standardTimeScale;
            paused = false;
        }
    }
}
