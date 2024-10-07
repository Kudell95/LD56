using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Scriptable Objects/Config")]
public class ConfigSO : ScriptableObject
{
    [Header("Player Configs")]
    public int StartingPlayerHealth = 50;

    public float MaxDodgePercentage = 0.3f;


    [Header("Difficulty")]
    public float DifficultyIncrement = 0.1f;

    public float MaxDifficultyModifier = 6f;

}
