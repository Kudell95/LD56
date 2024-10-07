using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    public int GameSceneIndex;
    
    private void Awake()
    {
		SoundManager.Instance?.PlayMusic("bgm01", true);
	}


	
	public void StartGame()
	{
		SoundManager.Instance?.PlaySound("select");
		SceneTransitionManager.Instance.LoadScene(GameSceneIndex);
	}
}
