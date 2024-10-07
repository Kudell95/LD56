using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    #region  Singleton
    public static GameManager Instance;
    private void Awake(){
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);              
    }

    #endregion

    public static ConfigSO Config {get {return Instance.ConfigObject; } }
    public ConfigSO ConfigObject;

    public TurnManager TurnManagerObject;
    public Player PlayerObject;

    public EnemyController EnemyObject;

    public int HealthCount, DefenseBuffCount, AttackBuffCount;


    private static int m_LevelCount;
    public static int LevelCount {
        get{ return m_LevelCount;}
        set{
            OnLevelCounterChanged?.Invoke(value);
            m_LevelCount = value;
        }
    }

    public Enums.InsectSpawnTypes GameSpawnType = Enums.InsectSpawnTypes.Random;

    public static Action CountsUpdated;

    public Sprite DefaultModifierIcon;
    public static bool ItemUsedThisRound = false;

    public  VictoryScreenManager VictoryScreen;

    public static bool BlockPausing = false;

    public static Action<int> OnLevelCounterChanged;

    public static bool SkipNextOpponentTurn = true;

    public static float DifficultyModifier = 0;


    public DeathScreen DeathUI;

    public List<EnemySO> LinearEnemySequence = new List<EnemySO>();

    public bool LastLinearEnemy {get{
        return LevelCount == LinearEnemySequence.Count-1;
    }}

    public bool IsPlayerTurn {get{
        return TurnManagerObject.CurrentTurnState == Enums.TurnStates.PlayerTurn;
    }}

    public bool IsOpponentTurn {get{
        return TurnManagerObject.CurrentTurnState == Enums.TurnStates.OpponentTurn;
    }}

    public static void EndTurn(){
        Instance.TurnManagerObject.EndTurn();
        
    }

    public static void StartTurn(Enums.TurnStates turn, bool notify = true){
        Instance.TurnManagerObject.StartTurn(turn,notify);
    }


    public void Start()
    {
        HealthCount = 1;
        DefenseBuffCount = 1;
        AttackBuffCount = 1;  
        TurnManagerObject.StartTurn(Enums.TurnStates.InitialTurn, false);
        TurnManager.OnTurnStart += OnTurnStart;
        LevelCount = 0;
    }

    public void OnTurnStart(Enums.TurnStates turn)
    {
        if(turn == Enums.TurnStates.OpponentDeadTurn)
            LevelCount++;
    }


    

    //for Linear playthrough
    public EnemySO GetNextEnemy()
    {
        if(LinearEnemySequence != null && LinearEnemySequence.Count > 0 && LevelCount < LinearEnemySequence.Count)
        {
            int currentlevel = LevelCount;
            return LinearEnemySequence[currentlevel];
        }

        return null;
    }

    private void OnDestroy() {
        TurnManager.OnTurnStart -= OnTurnStart;
    }

}
