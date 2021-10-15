using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have

    private bool braking=false;
    private float motor = 0;
    private float steering = 0;


    public void FixedUpdate()
    {
        /// For testing purposes
        //steering = Input.GetAxis("Horizontal");
        //motor = Input.GetAxis("Vertical");

        if (braking)
        {
            steering = 0;
            motor = 0;
        }

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering * maxSteeringAngle;
                axleInfo.rightWheel.steerAngle = steering * maxSteeringAngle;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor * maxMotorTorque;
                axleInfo.rightWheel.motorTorque = motor * maxMotorTorque;
                if (braking)
                {
                    axleInfo.leftWheel.brakeTorque = maxMotorTorque;
                    axleInfo.rightWheel.brakeTorque = maxMotorTorque;
                }
                else
                {
                    axleInfo.leftWheel.brakeTorque = 0;
                    axleInfo.rightWheel.brakeTorque = 0;
                }
            }
        }
        braking = false;
    }

    /// <summary>
    /// Should the motor apply torque?
    /// </summary>
    /// <param name="torque">1 = forward, -1 = backwards, 0 = no torque</param>
    public void ApplyMotorTorque(float torque)
    {
        this.motor = Mathf.Clamp(torque,-1f,1f);
    }

    /// <summary>
    /// Sets the desired steering angle
    /// </summary>
    /// <param name="steering">-1 = left, 1 = right, 0 forward</param>
    public void ApplySteering(float steering)
    {
        this.steering = Mathf.Clamp(steering,-1f,1f);
    }

    public void SetMaximumMotorTorque(float torque)
    {
        maxMotorTorque = torque;
    }

    public void SetMaximumSteeringAngle(float steering)
    {
        maxSteeringAngle = steering;
    }

    public void ApplyBrakes()
    {
        braking = true;
    }

}


[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}
