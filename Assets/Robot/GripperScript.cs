using UnityEngine;
using System.Collections;
using System;

public class GripperScript : MonoBehaviour {
    public float hingeAngle = 270;
    public JointMotor motorgripper;
    public HingeJoint hingegripper;
    public float speed, angleup, angledown;
    public int rotations;
    public static GripperScript main;

    // Use this for initialization
    void Awake() {
        main = this;
        hingegripper = GetComponent<HingeJoint>();
    }
    void Start() {

    }
    public float GetAngle(float PrevAngle) {
        Quaternion a = (Quaternion.Inverse(RobotController.main.transform.rotation) * transform.rotation);
        float difference = (a.eulerAngles.x + rotations * 360 - PrevAngle);
        if (Mathf.Abs(difference) > 180) {
            rotations -= Mathf.RoundToInt(Mathf.Sign(difference));
        }
        return a.eulerAngles.x + rotations * 360;
    }


    public void SetSpeed(float relspeed) {
        motorgripper = hingegripper.motor;
        motorgripper.targetVelocity = relspeed * speed;
        hingegripper.motor = motorgripper;
    }


    // Update is called once per frame
    void FixedUpdate() {
        hingeAngle = GetAngle(hingeAngle);
        //SetSpeed(Input.GetAxisRaw("Horizontal"));


    }
    public bool IsUp() {
        if (hingeAngle > angleup) {
            return true;
        } else return false;

    }

    public bool IsDown() {
        if (hingeAngle < angledown) {
            return true;
        } else return false;
    }
}