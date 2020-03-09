using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entry", menuName = "Menu Entry")]
public class MenuEntry : ScriptableObject
{
    [Tooltip("The audio clip that this menu entry will play. Leave blank to not play any audio")]
    public AudioClip clip;

    [Tooltip("The sub-menus of this menu")]
    public MenuEntry[] subMenus;

    [Tooltip("Whether to loop this clip after it finishes playing")]
    public bool LoopClip;

    [Tooltip("The name of a method to call on ")]
    public string StartMethod;
    public MenuEntry AutomaticEntry;
}