using UnityEngine;
using System.IO.Ports;
using System;

/// <summary>
/// A simple struct to hold a single state of the phone
/// </summary>
public struct PhoneState
{
    public int CurrentNumber; // The number just dialled
    public bool HandsetUp; // Whether the handset is in the cradle or not
}

[RequireComponent(typeof(PhoneListener))]
public class Arduino : MonoBehaviour
{
    public int commPort = 0; // The serial port that the Arduino is attached to

    private SerialPort serial = null; // The serial port that communications are occurring on

    public PhoneState state; // The state that the phone is currently in


    void Start()
    {
        for (int i = 2; i < 10; i++) // For all possible serial ports that the arduino could be on
        {
            try // and initiate communications on that port
            { 
                serial = new SerialPort("\\\\.\\COM" + i, 9600);
                serial.ReadTimeout = 50;
                serial.Open();
                commPort = i;
                break;
            }
            catch (System.IO.IOException)
            {
                //If there is an error, the correct comm port wasn't selected. Try the next one
            }            
        }
        Debug.Log("Found correct serial port: " + commPort); // Print a debug message to let us know that everything worked correctly
    }

    private void Update()
    {
        if (serial.BytesToRead > 12) // If there are more than 12 bytes to read (The length of "DialEvent: "), then there is probably a full line available
        {
            string Message = serial.ReadLine(); // Attempt to read the entire line
            if (Message.Contains("CradleEvent: ")) // If it's a cradle event
            {
                state.HandsetUp = Message.Split(' ')[1] == "Up"; // Set the cradle state to whether the state was "up"

                BroadcastMessage("CradleEvent", state); // Call the CradleEvent method on any sibling scripts / sibling objects / child objects
            }
            else if (Message.Contains("DialEvent")) // otherwise if it's a dial event
            {
                state.CurrentNumber = Convert.ToInt32(Message.Split(' ')[1]) % 10; // set the number dialled to the number recieved (mod 10 since a 0 is sent as 10)

                BroadcastMessage("DialEvent", state); // Call the DialEvent method on any sibling scripts / sibling objects / child objects
            }
        }
    }

    /// <summary>
    /// Called when the object is destroyed, either through the game being exited or a new scene being loaded
    /// </summary>
    void OnDestroy()
    {
        serial.Close(); // Close the serial port to stop all communications and free it up for later connections
    }
}