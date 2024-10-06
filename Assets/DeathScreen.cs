using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public GameObject DeathScreenParent;

    private void Start() {
        DeathScreenParent.SetActive(false);
    }
    public void Show(){
        Time.timeScale = 0;
        DeathScreenParent.SetActive(true);
    }

    public void Quit(){
        Time.timeScale = 1;
        SceneTransitionManager.Instance.LoadScene(Enums.Scenes.MainMenu);
    }
}
