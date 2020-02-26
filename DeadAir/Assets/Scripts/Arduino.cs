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
            try
            {
                string Message = serial.ReadLine();
                if (Message.Contains("CradleEvent: "))
                {
                    Debug.Log("Cradle event occurred");
                    if (Message.Split(' ')[1] == "Up")
                    {
                        state.HandsetUp = true;
                    }
                    else
                    {
                        state.HandsetUp = false;
                    }
                }
                else if (Message.Contains("DialEvent"))
                {
                    Debug.Log("Dial event occurred");
                    if (Message.Split(' ')[1] == "10")
                    {
                        state.CurrentNumber *= 10;
                    }
                    else
                    {
                        state.CurrentNumber = (state.CurrentNumber * 10) + Convert.ToInt32(Message.Split(' ')[1]);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    void OnDestroy()
    {
        Debug.Log("Exiting");
        serial.Close();
    }
}