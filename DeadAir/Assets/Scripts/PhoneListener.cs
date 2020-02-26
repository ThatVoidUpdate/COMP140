using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneListener : MonoBehaviour
{
    private MenuEntry CurrentMenuEntry;
    public MenuEntry TopLevelEntry;

    private float CurrentTime = 0;
    public float EntryTimeout = 2;

    private int CurrentInput = 0;

    public AudioSource Source;

    public int GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            return 0;
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            return 1;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            return 2;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            return 3;
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            return 4;
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            return 5;
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            return 6;
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            return 7;
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            return 8;
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            return 9;
        }

        return -1;
    }

    public void SwitchEntry(MenuEntry NewEntry)
    {
        CurrentMenuEntry = NewEntry;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTime += Time.deltaTime;

        int input = GetInput();
        if (input != -1)
        {
            CurrentInput = CurrentInput * 10 + input;
            CurrentTime = 0;
        }

        if (CurrentTime > EntryTimeout)
        {
            CurrentMenuEntry.DoMenu(CurrentInput);
            CurrentInput = 0;
        }
    }
}
