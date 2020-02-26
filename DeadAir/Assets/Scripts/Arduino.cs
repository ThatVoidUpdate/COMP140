using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using System.Collections;

public struct PhoneState
{
    public int ID;
    public int CurrentNumber;
    public bool HandsetUp;

    public PhoneState(int id)
    {
        ID = id;
        CurrentNumber = 0;
        HandsetUp = false;
    }
}

[RequireComponent(typeof(PhoneListener))]
public class Arduino : MonoBehaviour
{
    public int commPort = 0;

    private SerialPort serial = null;

    public PhoneState state = new PhoneState(0);

    // Use this for initialization
    void Start()
    {
        for (int i = 2; i < 10; i++)
        {
            try
            {
                serial = new SerialPort("\\\\.\\COM" + i, 9600);
                serial.ReadTimeout = 50;
                serial.Open();
                commPort = i;
                break;
            }
            catch (System.IO.IOException)
            {
                //We didnt select the correct com port, so try the next one
            }            
        }
        Debug.Log("Found correct serial port: " + commPort);
    }

    private void Update()
    {
        if (serial.BytesToRead > 12)
        {
            string Message = serial.ReadLine();
            if (Message.Contains("CradleEvent: "))
            {
                if (Message.Split(' ')[1] == "Up")
                {
                    state.HandsetUp = true;
                }
                else
                {
                    state.HandsetUp = false;
                }

                BroadcastMessage("CradleEvent", state);
            }
            else if (Message.Contains("DialEvent"))
            {
                if (Message.Split(' ')[1] == "10")
                {
                    state.CurrentNumber = 0;
                }
                else
                {
                    state.CurrentNumber = Convert.ToInt32(Message.Split(' ')[1]);
                }

                BroadcastMessage("DialEvent", state);
            }
        }
    }

    void OnDestroy()
    {
        Debug.Log("Exiting");
        serial.Close();
    }
}