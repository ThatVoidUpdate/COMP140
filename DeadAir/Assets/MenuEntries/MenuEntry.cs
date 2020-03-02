using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entry", menuName = "Menu Entry")]
public class MenuEntry : ScriptableObject
{
    public AudioClip clip;
    public MenuEntry[] subMenus;
    public bool LoopClip;
    public string StartMethod;
}