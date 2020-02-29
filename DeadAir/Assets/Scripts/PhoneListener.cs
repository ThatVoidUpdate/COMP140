using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Arduino))]
[RequireComponent(typeof(AudioSource))] // Make sure that the phone has an arduino and an audio source
public class PhoneListener : MonoBehaviour
{
    [Tooltip("The top level of all of the menus, which runs on game start")]
    public MenuEntry TopLevelEntry;     // ^
    private MenuEntry CurrentMenuEntry; // The current menu that the player is using

    public float GameStartDelay;        // The delay between the first time the user picks up the handset, and the first audio being played

    [Space]
    public AudioClip ErrorClip;         // The clip to be played if any kind of error occurs

    [Space]
    public GameObject DevMode;          // The gameobject which should be a parent of all of the dev mode ui items
    public Text log;                    // The text object holding the log of all acitons performed
    public bool DevmodeDefault;         // Whether to show the dev mode as default
    public Text CurrentNumberText;      // The text object used to show the current number dialled
    public Image PhoneUp;               // The image to show in dev mode when the handset is up
    public Image PhoneDown;             // The image to show in dev mode when the handset is down

    private string CurrentNumber;       // The current number that has been dialled by the user
    private AudioSource Source;         // The audio source attached to the controller, used to play all of the audio

    public void Start()
    {
        Source = GetComponent<AudioSource>(); // Get the audio source attached to the gameobject
        CurrentMenuEntry = TopLevelEntry; // Select the top level MenuEntry

        #if UNITY_EDITOR  // If in editor, and DevmodeDefault is true, activate dev mode on start. This code will be automatically removed when the game is built
        if (DevmodeDefault)
        {
            LaunchDevMode();
        }
        #endif
    }

    /// <summary>
    /// This event will be called whenever the state of the phone's cradle is changed (ie the handset is lifted or put down)
    /// </summary>
    /// <param name="state">The current state of the phone</param>
    public void CradleEvent(PhoneState state)
    {
        if (state.HandsetUp) // If this was a handset up event
        {   
            PhoneUp.gameObject.SetActive(true); // Enable the PhoneUp graphic
            PhoneDown.gameObject.SetActive(false); // Disable the PhoneDown graphic
            StartCoroutine(StartGame()); // Start the game
        }
        else
        {
            PhoneUp.gameObject.SetActive(false); // Disable the PhoneUp graphic
            PhoneDown.gameObject.SetActive(true); // Enable the PhoneDown graphic
            Source.Stop(); // Stop any audio playing through the audio source
            CurrentNumber = ""; // Clear the current number
            CurrentNumberText.text = ""; // Update the text
            StopAllCoroutines(); // Stop any coroutines that may be executing (mainly used for stopping any StartGame calls if the phone was put down before the GameStartDelay timeout)
        }
    }

    /// <summary>
    /// This event will be called whenever a number is dialled on the phone
    /// </summary>
    /// <param name="state">The current state of the phone</param>
    public void DialEvent(PhoneState state)
    { 
        Source.Stop(); // Stop any audio playing
        CurrentNumber = CurrentNumber + state.CurrentNumber; // Append the number dialled to the current number string
        CurrentNumberText.text = CurrentNumber.ToString(); // Update the number display
        SwitchMenu(state.CurrentNumber % 10); // Switch the menu. % 10 fixes the fact that 0 is dialled as 10
    }

    /// <summary>
    /// Called to start the game
    /// </summary>
    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(GameStartDelay); // Wait a short time for the phone to be put up to the user's ear
        log.text += "Starting Game\n"; // Add an event to the log
        CurrentMenuEntry = TopLevelEntry; // Return back to the top level of the menus
        Source.loop = CurrentMenuEntry.LoopClip; // Loop the audio is needed
        Source.clip = CurrentMenuEntry.clip; // Set the clip of the AudioSource
        Source.Play(); // Play the audio
    }

    /// <summary>
    /// Called to switch between different menus
    /// </summary>
    /// <param name="MenuEntry">The entry to switch to</param>
    public void SwitchMenu(int MenuEntry)
    {
        if (MenuEntry < CurrentMenuEntry.subMenus.Length && CurrentMenuEntry.subMenus[MenuEntry] != null) // Check that the menu entry we are trying to access actually exists
        {
            CurrentMenuEntry = CurrentMenuEntry.subMenus[MenuEntry]; // Switch the MenuEntry on this object to the new entry
            log.text += "Switched to new menu entry: " + CurrentMenuEntry.name + "\n"; // Add an event to the log

            if (CurrentMenuEntry.clip != null) // If there is audio attached to this entry
            {
                Source.clip = CurrentMenuEntry.clip; // Set the audio to the audio on the MenuEntry
                Source.loop = CurrentMenuEntry.LoopClip; // Loop the audio if necessary
                Source.Play(); // Play the audio
            }

            if (CurrentMenuEntry.StartMethod != "") // If there is a start method attached to this entry
            {
                BroadcastMessage(CurrentMenuEntry.StartMethod); // Run it on this object or any sibling/child objects
            }
        }
        else // If the entry doesnt exist
        {
            log.text += "Error occurred\n"; // Add an error message to the log
            Source.clip = ErrorClip; // Set the audio to the error clip
            Source.loop = false; // Disable looping
            Source.Play(); // Play the audio
        }
    }

    /// <summary>
    /// Used to display stats about the game, and access developer features
    /// </summary>
    public void LaunchDevMode()
    {
        DevMode.gameObject.SetActive(true); // Showe all of the ui elements
        log.text += "Launched dev mode\n"; // Append and event to the log
    }

    /// <summary>
    /// Clears the event log in dev mode
    /// </summary>
    public void ClearLog()
    {
        log.text = ""; // clear the log
    }

    /// <summary>
    /// Used to quit the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit(); // Quit the game
    }


}

//#7582♥