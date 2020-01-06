using UnityEngine;
using System.Collections.Generic;
using System;

public class RobotController : MonoBehaviour {
    public enum Colors { black, gray, white, nothing }
    public float resetLeftAngle, resetRightAngle, leftAngle, rightAngle;
    public int rotationsL, rotationsR;
    public float correctedLeftAngle, correctedRightAngle, targetAngle = 0;
    private float minAngle = 7, kP = 0.4f, kC = 0.3f;
    private JointMotor motor1, motor2;
    private HingeJoint hinge1, hinge2;
    public float maxspeed, lightsensorangle;
    public Vector3 sensorpos;
    public float maxsensordist;
    public static RobotController main;
    public bool usingrightmotor, rotating;
    public Transform rwheel, lwheel;
    // Use this for initialization
    void Awake() {
        main = this;
        hinge1 = GetComponents<HingeJoint>()[0];
        hinge2 = GetComponents<HingeJoint>()[1];
    }

    public bool IsFinished(float endAngle) {
        if (usingrightmotor && Mathf.Abs(correctedLeftAngle) > endAngle) {
            return true;
        } else if (Mathf.Abs(correctedRightAngle) > endAngle) {
            return true;
        } else return false;
    }

    public void StopActions() {
        rotating = false;
        SetSpeed(0, 0);
    }

    public void SetSpeed(float leftspeed, float rightspeed) {
        //print("SetSPEED: " + leftspeed.ToString() + ' ' + rightspeed.ToString());
        motor1 = hinge1.motor;
        motor2 = hinge2.motor;
        motor1.targetVelocity = Mathf.Clamp(leftspeed * maxspeed, -maxspeed, maxspeed);
        motor2.targetVelocity = Mathf.Clamp(rightspeed * maxspeed, -maxspeed, maxspeed);
        hinge1.motor = motor1;
        hinge2.motor = motor2;
        //print(hinge2.angle);
    }
    public void SetSpeedSteering(float speed, float steering) {
        float leftspeed, rightspeed;
        usingrightmotor = (steering > 0);
        if (steering > 0) {
            leftspeed = speed;
            rightspeed = speed * (1 - steering * 2);
        } else {
            rightspeed = speed;
            leftspeed = speed * (1 + steering * 2);
        }
        SetSpeed(leftspeed, rightspeed);
    }
    public float CurrentLeftAngle(float PrevAngle) {
        Quaternion a = (Quaternion.Inverse(lwheel.rotation) * transform.rotation);
        float difference = (a.eulerAngles.y + rotationsL * 360 - PrevAngle);
        if (Mathf.Abs(difference) > 180) {
            rotationsL -= Mathf.RoundToInt(Mathf.Sign(difference));
        }
        return a.eulerAngles.y + rotationsL * 360;
    }
    public float CurrentRightAngle(float PrevAngle) {
        Quaternion a = (Quaternion.Inverse(rwheel.rotation) * transform.rotation);
        float difference = (a.eulerAngles.y + rotationsR * 360 - PrevAngle);
        if (Mathf.Abs(difference) > 180) {
            rotationsR -= Mathf.RoundToInt(Mathf.Sign(difference));
        }
        return a.eulerAngles.y + rotationsR * 360;
    }

    public void ResetWheelAngles() {
        resetLeftAngle = leftAngle;
        resetRightAngle = rightAngle;
    }

    public bool RotateTotargetAngle() {
        float difference = (((transform.rotation.eulerAngles.y - targetAngle) / 360 + 0.5f) % 1) - 0.5f;
        //print(difference*360);
        if (Mathf.Abs(difference) < minAngle / 360) {
            return true;
        }
        SetSpeedSteering(difference * kP + Mathf.Sign(difference) * kC, 1);
        return false;
    }

    public Colors RayCastSensor(float angle) {
        Vector3 origin = transform.position + transform.rotation * sensorpos;
        Vector3 direction = -transform.forward + Mathf.Tan(angle) * transform.right;
        if (Physics.Raycast(origin, direction, maxsensordist, LayerMask.GetMask("Black"))) {
            return Colors.black;
        } else if (Physics.Raycast(origin, direction, maxsensordist, LayerMask.GetMask("Gray"))) {
            return Colors.gray;
        } else if (Physics.Raycast(origin, direction, maxsensordist, LayerMask.GetMask("White"))) {
            return Colors.white;
        } else {
            return Colors.nothing;
        }
    }
    public List<Colors> lightsensor() {
        List<Colors> outputlist = new List<Colors>();
        outputlist.Add(RayCastSensor(0));
        outputlist.Add(RayCastSensor(lightsensorangle));
        outputlist.Add(RayCastSensor(-lightsensorangle));
        return outputlist;
    }
    // Update is called once per frame
    void FixedUpdate() {
        if (rotating) {
            if (RotateTotargetAngle()) {
                rotating = false;
                SetSpeed(0, 0);
            }
        }
        leftAngle = CurrentLeftAngle(leftAngle);
        correctedLeftAngle = leftAngle - resetLeftAngle;
        rightAngle = CurrentRightAngle(rightAngle);
        correctedRightAngle = rightAngle - resetRightAngle;
        //print(lightsensor());
    }
    private void Update() {
        Debug.DrawRay(transform.position + transform.rotation * sensorpos, -transform.forward * maxsensordist, Color.red, 0, true);
        Debug.DrawRay(transform.position + transform.rotation * sensorpos, (-transform.forward + Mathf.Tan(lightsensorangle) * transform.right).normalized * maxsensordist, Color.red, 0, true);
        Debug.DrawRay(transform.position + transform.rotation * sensorpos, (-transform.forward + Mathf.Tan(-lightsensorangle) * transform.right).normalized * maxsensordist, Color.red, 0, true);
    }

}
