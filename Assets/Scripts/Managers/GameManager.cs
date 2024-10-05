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


    public Enums.InsectSpawnTypes GameSpawnType = Enums.InsectSpawnTypes.Random;


    public static void EndTurn(){
        Instance.TurnManagerObject.EndTurn();
    }

    public static void StartTurn(Enums.TurnStates turn){
        Instance.TurnManagerObject.StartTurn(turn);
    }


    public void Start()
    {
        TurnManagerObject.StartTurn(Enums.TurnStates.InitialTurn, false);
    }




}
