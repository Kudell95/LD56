using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDB", menuName = "Scriptable Objects/EnemyDB")]
public class EnemyDB : ScriptableObject
{    
    public WeightedEnemyList EnemyList;

}


public class WeightedEnemyList : UnitySerializedDictionary<EnemySO, float>{}