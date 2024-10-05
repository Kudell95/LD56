
using UnityEngine;

public class Player : MonoBehaviour
{
    private int CurrentHealth;

    private int CurrentMaxHealth;

    public bool Dead;

    private void Awake()
    {
        TurnManager.OnTurnStart += OnTurnStarted;
        TurnManager.OnTurnEnd += OnTurnEnded;
    }


    private void OnTurnStarted(Enums.TurnStates turnType)
    {
    }

    private void OnTurnEnded(Enums.TurnStates turnType)
    {

    }

    //this will probably go in an event somewhere.
    public void InitPlayer()
    {
        CurrentHealth = GameManager.Config.StartingPlayerHealth;
        CurrentMaxHealth = GameManager.Config.StartingPlayerHealth;
        Dead = false;
    }



    private void OnDestroy() 
    {
        TurnManager.OnTurnStart -= OnTurnStarted;
        TurnManager.OnTurnEnd -= OnTurnEnded;    
    }



}
