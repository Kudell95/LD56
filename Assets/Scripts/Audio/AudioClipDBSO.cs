
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDB", menuName = "Audio/New audio clip db", order = 1)]
public class AudioClipDBSO : ScriptableObject
{
	public AudioClipDictionary AudioClips = new ();
}

[Serializable]
public class AudioClipDictionary : UnitySerializedDictionary<string, AudioClip> { }
