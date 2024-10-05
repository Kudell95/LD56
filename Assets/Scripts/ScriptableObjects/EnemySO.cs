using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public string Name;
    [Multiline]
    public string description;

    public int Health;

    public int Armour;

    public Enums.OpponentDifficulty Difficulty;

    //TODO: add ability sets here, should be simple collection of attack SO - enum ability type with value amount and duration (optional).

    public WeightedAbilityList AbilityList;


    public EnemyAbilitySO GetRandomAbility(){
       return AbilityList.RandomElementByWeight(x=> x.Value).Key.Clone();
    }
    
}

[Serializable]
public class WeightedAbilityList : UnitySerializedDictionary<EnemyAbilitySO,float>{}