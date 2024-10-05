using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Scriptable Objects/Config")]
public class ConfigSO : ScriptableObject
{
    [Header("Player Configs")]
    public int StartingPlayerHealth = 20;

    

}
