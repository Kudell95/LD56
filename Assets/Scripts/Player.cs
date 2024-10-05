
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private int CurrentHealth;

    private int CurrentMaxHealth;


    public List<PlayerAbilitySO> AbilityList = new List<PlayerAbilitySO>();

    public List<PlayerAbilitySO> ItemList = new List<PlayerAbilitySO>();
    public bool Dead;

    private void Awake()
    {
        TurnManager.OnTurnStart += OnTurnStarted;
        TurnManager.OnTurnEnd += OnTurnEnded;
    }


    private void OnTurnStarted(Enums.TurnStates turnType)
    {
        if(turnType != Enums.TurnStates.InitialTurn && turnType != Enums.TurnStates.PlayerTurn)
            return;


        switch(turnType)
        {
            case Enums.TurnStates.InitialTurn:
            {
                InitPlayer();
                return;
            }
        }

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
        OnSpawn(()=>{
            GameManager.Instance.TurnManagerObject.EndTurn();
        });
    }


    public void OnSpawn(TweenCallback oncomplete)
    {
        float startingScaleY = transform.localScale.y;
        transform.DOScaleY(0,0f).OnComplete(()=>{
            transform.DOScaleY(startingScaleY,1f).SetEase(Ease.OutBack).OnComplete(oncomplete);
        });
    }



    private void OnDestroy() 
    {
        TurnManager.OnTurnStart -= OnTurnStarted;
        TurnManager.OnTurnEnd -= OnTurnEnded;    
    }



}
