using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
	public static SceneTransitionManager Instance { get; private set; }
	public Image ScreenFader;
	

	public void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);

		DontDestroyOnLoad(this);
		ScreenFader.gameObject.SetActive(false);
	}


	public void LoadScene(int sceneIndex, float duration = 0.5f, bool fadeout = true, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
	{      
		ScreenFader.gameObject.SetActive(true);
		ScreenFader.color = new Color(0, 0, 0, 0);
		LeanTween.color(ScreenFader.rectTransform, new Color(0, 0, 0, 1), duration).setOnComplete(() =>
		{
			SceneManager.LoadScene(sceneIndex, loadSceneMode);
			if(fadeout)
			{
				LeanTween.color(ScreenFader.rectTransform, new Color(0, 0, 0, 0), duration).setOnComplete(() =>
				{
					ScreenFader.gameObject.SetActive(false);
				});
			}
		});
	}
	
	public void FadeOut(float duration = 0.5f)
	{
		if(!ScreenFader.gameObject.activeSelf)
			return;
			
		LeanTween.color(ScreenFader.rectTransform, new Color(0, 0, 0, 0), duration).setOnComplete(() =>
		{
			ScreenFader.gameObject.SetActive(false);
		});
	}
	
	
	public void LoadScene(int sceneIndex, Action OnComplete, float duration = 0.5f, bool fadeout = true, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
	
	{
		if(!ScreenFader.gameObject.activeSelf){			
			ScreenFader.gameObject.SetActive(true);
			ScreenFader.color = new Color(0, 0, 0, 0);
		}
		LeanTween.color(ScreenFader.rectTransform, new Color(0, 0, 0, 1), duration).setOnComplete(() =>
		{
			SceneManager.LoadScene(sceneIndex,loadSceneMode);
			LeanTween.delayedCall(0.1f,()=> 
			{
				OnComplete?.Invoke();
				if(fadeout)
				{
					LeanTween.color(ScreenFader.rectTransform, new Color(0, 0, 0, 0), duration).setOnComplete(() =>
					{
						ScreenFader.gameObject.SetActive(false);					
					});
				}				
			});			
			
		});
	}
	
	public void LoadScene(Enums.Scenes scene, Action onCompleteEvent, float duration = 0.5f, bool fadeout = true, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
	{      
		LoadScene((int)scene, onCompleteEvent, duration, fadeout, loadSceneMode);
	}
	
	public void LoadScene(Enums.Scenes scene, float duration = 0.5f, bool fadeout = true, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
	{      
		LoadScene((int)scene, duration, fadeout, loadSceneMode);
	}
	
	public void UnLoadScene(Enums.Scenes scene, float duration = 0.5f)	
	{
		UnLoadScene((int)scene, duration);
	}
	
	public void UnLoadScene(int sceneIndex, float duration = 0.5f)
	{
		ScreenFader.gameObject.SetActive(true);
		ScreenFader.color = new Color(0, 0, 0, 0);
		LeanTween.color(ScreenFader.rectTransform, new Color(0, 0, 0, 1), duration).setOnComplete(() =>
		{
			SceneManager.UnloadSceneAsync(sceneIndex);
			SceneManager.sceneUnloaded += (scene) =>
			{	
				LeanTween.color(ScreenFader.rectTransform, new Color(0, 0, 0, 0), duration).setOnComplete(() =>
				{
					ScreenFader.gameObject.SetActive(false);
				});
			};
		});		
	}
	

	
	

	public void FadeToEvent(float duration, Action OnComplete)
	{
		ScreenFader.gameObject.SetActive(true);
		ScreenFader.color = new Color(0, 0, 0, 0);
		LeanTween.color(ScreenFader.rectTransform, new Color(0, 0, 0, 1), duration).setOnComplete(() =>
		{
			LeanTween.delayedCall(duration / 4, () =>
			{
				OnComplete();
				LeanTween.color(ScreenFader.rectTransform, new Color(0, 0, 0, 0), 0.5f).setOnComplete(() =>
				{
					ScreenFader.gameObject.SetActive(false);
				});
			});
		   
		});
	}


}
