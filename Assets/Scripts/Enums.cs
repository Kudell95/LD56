using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{	
	public enum Scenes
	{
		MainMenu = 1,
		GameScene = 2,
		LoadingScene = 3,
	}
	
	// public enum Rarity
	// {
	// 	Common,
	// 	Rare,
	// 	Epic,
	// 	Legendary
	// }
	
	
	public enum AbilityType
	{
		Attack,
		Heal,
		DefenceBuff,
		AttackRange,
		TurnSkip,
		AttackBuff,
        HealForRound,
    }
	
	public enum BroadAbilityType
	{
		Attack,
		Heal,
		Buff,
		Debuff,
	}
	
	public enum AbilityActionType
	{
		Place,
		Discard
	}

	public enum OpponentDifficulty
	{
		Worker = 1,
		Fighter,
		Champion,
		Boss
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
