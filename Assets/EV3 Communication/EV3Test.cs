using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Lego.Ev3.Core;
//using Lego.Ev3.Desktop;
using System;

public class EV3Test : MonoBehaviour
{
    //Brick brick;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    //brick = new Brick(new BluetoothCommunication("COM18"));
    //    brick = new Brick(new UsbCommunication());
    //    brick.BrickChanged += Brick_Changed;
    //    BrickConnect();
    //}
    //async void BrickConnect() {
    //    await brick.ConnectAsync();
    //    await brick.DirectCommand.PlayToneAsync(30, 440, 100);
    //    brick.Ports[InputPort.A].SetMode(MotorMode.Degrees);
    //    brick.BatchCommand.StepMotorAtSpeed(OutputPort.A, 20, 360, true);
    //    //brick.BatchCommand.StepMotorAtSpeed(OutputPort.B, 40, 360, true);
    //    await brick.BatchCommand.SendCommandAsync();
    //    //for (int i = 1; i < 1000; i++) {
    //    //    Debug.Log(brick.Ports[InputPort.A].RawValue);
    //    //}
    //}
    //private void Brick_Changed(object sender, BrickChangedEventArgs e) {
    //    Debug.Log("Brick Changed!");
    //    Debug.Log(e.Ports[InputPort.One].SIValue);
    //    //Debug.Log(e.ToString);
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
