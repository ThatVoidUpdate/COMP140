using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEntry : MonoBehaviour
{
    public Dictionary<int, MenuEntry> ChildItems;
    public AudioClip Audio;

    public PhoneListener listener;


    public void DoMenu(int input)
    {
        if (ChildItems.ContainsKey(input))
        {
            ChildItems[input].Play();
        }
    }

    public void Play()
    {
        listener.SwitchEntry(this);
        listener.Source.clip = Audio;
        listener.Source.Play();
    }
}
