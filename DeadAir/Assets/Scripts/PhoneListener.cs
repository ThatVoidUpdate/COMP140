using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Arduino))]
public class PhoneListener : MonoBehaviour
{
    private MenuEntry CurrentMenuEntry;
    public MenuEntry TopLevelEntry;

    private float CurrentTime = 0;
    public float EntryTimeout = 2;

    public string CurrentNumber;

    public Image PhoneUp;
    public Image PhoneDown;
    public Text CurrentNumberText;

    public AudioClip[] clips;

    public AudioSource Source;

    public void CradleEvent(PhoneState state)
    {
        if (state.HandsetUp)
        {
            PhoneUp.gameObject.SetActive(true);
            PhoneDown.gameObject.SetActive(false);
            Source.Play();
        }
        else
        {
            PhoneUp.gameObject.SetActive(false);
            PhoneDown.gameObject.SetActive(true);
            Source.Stop();
            CurrentNumber = "";
            CurrentNumberText.text = CurrentNumber.ToString();
            Source.clip = clips[0];
        }
    }

    public void DialEvent(PhoneState state)
    {
        Source.Stop();
        CurrentNumber = CurrentNumber + state.CurrentNumber;
        CurrentNumberText.text = CurrentNumber.ToString();

        if (CurrentNumber == "9999")
        {
            Source.clip = clips[1];
            Source.Play();
        }
    }

    public void SwitchEntry(MenuEntry NewEntry)
    {
        CurrentMenuEntry = NewEntry;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
