using UnityEngine;

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


    public void Start()
    {
        
    }




}
