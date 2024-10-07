using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemDB", menuName = "Scriptable Objects/ShopItemDB")]
public class ShopItemDB : ScriptableObject
{
    public WeightedShopItemList ShopItemList;
}


[Serializable]
public class WeightedShopItemList : UnitySerializedDictionary<ShopItemSO, float>
{

    public WeightedShopItemList Clone(){
        return (WeightedShopItemList)MemberwiseClone();
    }

}