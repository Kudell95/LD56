using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    public static Action<Enums.TurnStates> OnTurnStart;
    public static Action<Enums.TurnStates> OnTurnEnd;

    public Enums.TurnStates CurrentTurnState;

    public void StartTurn(Enums.TurnStates turn, bool notify = true)
    {
        if(CurrentTurnState == Enums.TurnStates.PlayerDeadTurn)
			return;
		
        CurrentTurnState = turn;

        OnTurnStart?.Invoke(turn);
        
        if(notify)
            NotificationManager.Instance?.Notify(GetTurnText(turn));

    }

    public void EndTurn(Enums.TurnStates nextTurn = Enums.TurnStates.None)
    {
        if(nextTurn == Enums.TurnStates.None)
            nextTurn = GetNextTurn(CurrentTurnState);
        
        OnTurnEnd?.Invoke(CurrentTurnState);
        StartTurn(nextTurn, CurrentTurnState != Enums.TurnStates.InitialTurn);
    }

    public Enums.TurnStates GetNextTurn(Enums.TurnStates currentTurn)
    {

        switch (currentTurn)
        {
            case Enums.TurnStates.PlayerTurn:
                return Enums.TurnStates.OpponentTurn;

            case Enums.TurnStates.OpponentTurn:
                return Enums.TurnStates.PlayerTurn;

            case Enums.TurnStates.OpponentDeadTurn:
                return Enums.TurnStates.OpponentSpawnTurn;

            case Enums.TurnStates.OpponentSpawnTurn:
                return Enums.TurnStates.PlayerTurn;

            case Enums.TurnStates.InitialTurn:
                return Enums.TurnStates.OpponentSpawnTurn;

            default:
                return Enums.TurnStates.PlayerTurn;            
        }
    }

    
    
    public string GetTurnText(Enums.TurnStates turnStates)
	{
		switch(turnStates)
		{
			case Enums.TurnStates.PlayerTurn:
				return "Player's Turn";
			case Enums.TurnStates.OpponentTurn:
                if(GameManager.SkipNextOpponentTurn)
                    return "Opponent stunned, Skipping turn...";
                else
				    return "Opponent's Turn";
            case Enums.TurnStates.OpponentDeadTurn:
                return "Bug destroyed. Hunting for next opponent...";
			case Enums.TurnStates.OpponentSpawnTurn:
				return "Prey found!";
			default:
				return "";
		}
	}

}
