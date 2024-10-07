using System;
using UnityEngine;

public class Modifier
{
    public string Name;
    public string Description;
    public string EffectDescription;

    /// <summary>
    /// will be deleted after the enemy is killed
    /// </summary>
    public bool TemporaryModifier; 

    public int Amount;

    public bool UseRange;
    public int MinAmount;
    public int MaxAmount;

    public Enums.ModifierTypes ModifierType;

    public Sprite ModifierIcon;

    
    
}
