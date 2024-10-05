using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DEBUG_Helper : MonoBehaviour 
{
	public void Init()
	{
		StartSceneID = SceneManager.GetActiveScene().buildIndex;
		DontDestroyOnLoad(this);
	}
	public static int StartSceneID = -1;
}
