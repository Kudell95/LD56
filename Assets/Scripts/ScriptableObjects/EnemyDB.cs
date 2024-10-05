using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDB", menuName = "Scriptable Objects/EnemyDB")]
public class EnemyDB : ScriptableObject
{    
    public WeightedEnemyList EnemyList;

    public EnemySO GetRandomEnemy(){
        return EnemyList.RandomElementByWeight(x => x.Value).Key.Clone();
    }

}

[Serializable]
public class WeightedEnemyList : UnitySerializedDictionary<EnemySO, float>{}