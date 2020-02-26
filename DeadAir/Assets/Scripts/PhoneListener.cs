﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Arduino))]
public class PhoneListener : MonoBehaviour
{
    private MenuEntry CurrentMenuEntry;
    public MenuEntry TopLevelEntry;       

    [Space]
    public Image PhoneUp;
    public Image PhoneDown;

    [Space]
    public Text CurrentNumberText;
    private string CurrentNumber;

    public float GameStartDelay;
    public float EntryTimeout = 2;

    [Space]
    public AudioSource Source;

    public AudioClip ErrorClip;

    [Space]
    public GameObject DevMode;

    public void CradleEvent(PhoneState state)
    {
        if (state.HandsetUp)
        {
            PhoneUp.gameObject.SetActive(true);
            PhoneDown.gameObject.SetActive(false);
            StartCoroutine(StartGame());
        }
        else
        {
            PhoneUp.gameObject.SetActive(false);
            PhoneDown.gameObject.SetActive(true);
            Source.Stop();
            CurrentNumber = "";
            CurrentNumberText.text = CurrentNumber.ToString();
        }
    }

    public void DialEvent(PhoneState state)
    {
        Source.Stop();
        CurrentNumber = CurrentNumber + state.CurrentNumber;
        CurrentNumberText.text = CurrentNumber.ToString();
        SwitchMenu(state.CurrentNumber % 10);
    }

    public void SwitchEntry(MenuEntry NewEntry)
    {
        CurrentMenuEntry = NewEntry;
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(GameStartDelay);
        CurrentMenuEntry = TopLevelEntry;
        Source.loop = CurrentMenuEntry.LoopClip;
        Source.clip = CurrentMenuEntry.clip;
        Source.Play();
    }

    public void SwitchMenu(int MenuEntry)
    {
        if (MenuEntry < CurrentMenuEntry.subMenus.Length && CurrentMenuEntry.subMenus[MenuEntry] != null)
        {
            
            CurrentMenuEntry = CurrentMenuEntry.subMenus[MenuEntry];

            if (CurrentMenuEntry.clip != null)
            {
                Source.clip = CurrentMenuEntry.clip;
                Source.loop = CurrentMenuEntry.LoopClip;
                Source.Play();
            }

            if (CurrentMenuEntry.StartMethod != "")
            {
                BroadcastMessage(CurrentMenuEntry.StartMethod);
            }
        }
        else
        {
            Source.clip = ErrorClip;
            Source.loop = false;
            Source.Play();
        }
    }

    public void LaunchDevMode()
    {
        DevMode.gameObject.SetActive(true);
    }


}
