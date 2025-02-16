using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool Active;

    public GameObject PauseParent;

    private void Start() {
        Time.timeScale = 1;
        PauseParent.SetActive(false);
        Active = false;
    }


    public void Pause()
    {
        PauseParent.SetActive(true);
        Time.timeScale = 0;
        Active = true;
    }
    
    public void Resume()
    {
        Time.timeScale = 1;
        PauseParent.SetActive(false);
        Active = false;
        SoundManager.Instance?.PlaySound("select");
    }

    public void Quit()
    {
        Time.timeScale = 1;
        SceneTransitionManager.Instance.LoadScene(Enums.Scenes.MainMenu);
        SoundManager.Instance?.PlaySound("cancel");

    }


    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Active)
                Resume();
            else
                if(!GameManager.BlockPausing)
                    Pause();
        }

        if(Active && GameManager.BlockPausing)
            Resume();
    }

}
