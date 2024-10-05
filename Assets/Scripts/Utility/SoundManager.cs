using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	
	[SerializeField]
   	private AudioSource MusicSource;
	[SerializeField]
   	private AudioSource EffectSource;
	[SerializeField]
   	private AudioSource AtmosphereSource;
	
	public AudioClipDBSO AudioClipDB;
   
  	public static SoundManager Instance;
   
   	private void Awake() 
	{
		if(Instance == null)
		{
			Instance = this;
		}else
		{
			Destroy(this);
		}
		
   }   
   
	// Plays a sound using the provided AudioClip.
	public void PlaySound(AudioClip _AudioClip)
	{
		EffectSource.PlayOneShot(_AudioClip);
	}
	
	// Play a sound specified by the given audio clip name.
	// 
	// Parameters:
	//   _AudioClipName - The name of the audio clip to be played.
	// 
	// Returns:
	//   void
	public void PlaySound(string _AudioClipName)
	{
		if(!AudioClipDB.AudioClips.ContainsKey(_AudioClipName))
			return;
		
		
		AudioClip clip = AudioClipDB.AudioClips[_AudioClipName];
		
		if(clip == null)
		{
			Debug.LogWarning("Unable to find audio clip");
			return;
		}
		
		PlaySound(clip);
	}
	
	/// <summary>
	/// Plays the specified music clip.
	/// </summary>
	/// <param name="_MusicClip">The audio clip to play.</param>
	/// <param name="repeat">(Optional) Whether to repeat the music clip. Defaults to false.</param>
	public void PlayMusic(AudioClip _MusicClip, bool repeat = false)
	{
		MusicSource.clip = _MusicClip;	
		MusicSource.loop = repeat;	
		MusicSource.Play();
	}
	
	// Plays the music with the given _MusicClipName and optional repeat flag.
	public void PlayMusic(string _MusicClipName, bool repeat = false)
	{

		if(!AudioClipDB.AudioClips.ContainsKey(_MusicClipName))
		{
			Debug.LogWarning("Clip name doesn't exist in audio db");
			return;
		}

		AudioClip clip = AudioClipDB.AudioClips[_MusicClipName];
		
		if(clip == null)
		{
			Debug.LogWarning("Unable to find audio clip");
			return;
		}
		
		PlayMusic(clip, repeat);
		
	}

	public void ToggleMute()
	{
		if (AudioListener.volume == 0)
		{
			Unmute();
		}
		else
		{
			AudioListener.volume = 0;
			Debug.Log("Muted Audio!");
		}
	}

	public void Unmute()
	{
		AudioListener.volume = 1;
		Debug.Log("Unmuted Audio!");
	}
   
}
