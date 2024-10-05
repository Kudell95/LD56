using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Debug_Loader : MonoBehaviour
{
	
	public GameObject DebugHelperPrefab;
	// Start is called before the first frame update
	private void Awake()
	{
#if UNITY_EDITOR
		//Random Singleton, to check if the starting scene has been run...			
		if(SoundManager.Instance == null)
		{
			var go = Instantiate(DebugHelperPrefab);
			go.GetComponent<DEBUG_Helper>().Init();
			SceneManager.LoadScene(0);
		}
#else
		Destroy(this);
#endif	
		
	}

}
