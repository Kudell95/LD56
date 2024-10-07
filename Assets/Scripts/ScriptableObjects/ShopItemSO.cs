using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemSO", menuName = "Scriptable Objects/ShopItemSO")]
public class ShopItemSO : ScriptableObject
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

    public Enums.ShopItemType ShopItemType;

    public Enums.ModifierTypes ModifierType;
    

    public Sprite DisplayImage;

    public Sprite ModifierImage;


    public bool SingleUse;
    
    public ShopItemSO Clone()
    {
        return (ShopItemSO)MemberwiseClone();
    }

}
