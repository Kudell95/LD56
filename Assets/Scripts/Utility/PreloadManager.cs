using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadManager : MonoBehaviour
{
	public int NextSceneIndex = 1;
	public int UISceneIndex = 2;
	void Start()
	{
		
		#if UNITY_EDITOR
			if(DEBUG_Helper.StartSceneID > 0)
				NextSceneIndex = DEBUG_Helper.StartSceneID;
		#endif
		SceneManager.LoadScene(NextSceneIndex);
		// SceneManager.LoadScene(UISceneIndex, LoadSceneMode.Additive);
		
		
	}

}
