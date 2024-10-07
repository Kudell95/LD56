using UnityEngine;

public class VictoryScreenManager : MonoBehaviour
{
    public GameObject VictoryScreenParent;

    private void Start() {
        VictoryScreenParent.SetActive(false);
    }
    public void Show(){
        Time.timeScale = 0;
        VictoryScreenParent.SetActive(true);
        GameManager.BlockPausing = true;
    }

    public void Hide()
    {
        Time.timeScale = 1;
        VictoryScreenParent.SetActive(false);
        GameManager.BlockPausing = false;
    }

    public void Continue(){
        GameManager.Instance.GameSpawnType = Enums.InsectSpawnTypes.Random;
        LeanTween.delayedCall(1f,()=>{
            GameManager.Instance.PlayerObject.HealToMaxHealth();
            GameManager.EndTurn();
        });
        Hide();
    }

    public void Quit(){
        Time.timeScale = 1;
        SceneTransitionManager.Instance.LoadScene(Enums.Scenes.MainMenu);
    }
}
