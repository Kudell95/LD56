using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAbilitySO", menuName = "Scriptable Objects/EnemyAbilitySO")]
public class EnemyAbilitySO : ScriptableObject
{
    public Enums.OpponentAbilityCategory AbilityCategory;
    public Enums.OpponentAbilityType AbilityType;

    
    [Tooltip("The value that will be applied when performing the action i.e. x damage if an attack.")]    
    public int value;
    [Tooltip("The number of rounds this will be applied")]
    public int duration;


    public EnemyAbilitySO Clone()
	{
		return (EnemyAbilitySO)MemberwiseClone();
    }
	
}
