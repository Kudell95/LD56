using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    public int GameSceneIndex;
    
    private void Awake()
    {
		SoundManager.Instance?.PlayMusic("MenuTheme", true);
	}
	
	public void StartGame()
	{
		SoundManager.Instance?.PlaySound("ButtonClick");
		SceneTransitionManager.Instance.LoadScene(GameSceneIndex);
	}
}
