using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAbilitySO", menuName = "Scriptable Objects/PlayerAbilitySO")]
public class PlayerAbilitySO : ScriptableObject
{
    public string Name;
    [Multiline]
    public string Description;
    [Multiline]
    public string EffectDescription;
    

    public bool UseRange;

    [HideIf("UseRange")]
    public int Amount;

    [ShowIf("UseRange")]
    public int MinAmount;
    [ShowIf("UseRange")]
    public int MaxAmount;

    [Tooltip("Effect lasts for a single fight.")]
    public bool SingleFightEffect;


    // public bool HasDuration;

    public Enums.ActionTypes ActionType;

    public Enums.AbilityType AbilityType;
    public Sprite AbilityImage;


    public Sprite ModifierImage;

}
