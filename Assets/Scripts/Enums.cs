using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{	
	public enum Scenes
	{
		MainMenu = 1,
		GameScene = 2,
	}
		
	
	public enum AbilityType
	{
		Attack,
		Heal,
		DefenceBuff,
		AttackRange,
		AttackBuff
    }	

	public enum ActionTypes
	{
		Ability,
		Item
	}

	public enum AttackAbilityNames{
		None,
		Slash,
		Vice,
		Sting
	}
	

	public enum OpponentDifficulty
	{
		Worker = 1,
		Fighter,
		Champion,
		Boss
	}

	public enum ModifierTypes
	{
		DefenceBoost,
		AttackBoost,
		Heal,
		SlashPoisonDamage,
		VicePoisonDamage,
		StingPoisonDamage,
		SlashLifeSteal,
		ViceLifeSteal,
		StingLifeSteal,
		DodgePercentage,
		MaxHealth,
		HealAfterFight
		
	}

	public enum ShopItemType{
		Item,
		Modifier
	}

	public enum OpponentAbilityType // WIP
	{
		BasicAttack,
		Retreat,
		Heal,
		Poison,
		Defend
	}
	
	public enum OpponentAbilityCategory // WIP
	{
		Attack,
		Heal,
		Buff
	}

	public enum InsectSpawnTypes {
		Sequential,
		Random
	}

	
	public enum TurnStates
	{
		None,
		InitialTurn,
		PlayerTurn,
		OpponentTurn,
		OpponentSpawnTurn,
		OpponentDeadTurn,
		PlayerDeadTurn,
		VictoryTurn,		
	}
}
