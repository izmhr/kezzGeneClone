using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneInput : MonoBehaviour
{
	public float sensitivity = 100;
	public float loudness = 0;
	private float lastLoudness = 0;

	void Awake()
	{
		audio.clip = Microphone.Start(null, true, 10, 44100);
		audio.loop = true; // Set the AudioClip to loop
		audio.mute = true; // Mute the sound, we don't want the player to hear it
		while (!(Microphone.GetPosition(null) > 0)) { } // Wait until the recording has started
		audio.Play(); // Play the audio source!

	}

	void Start()
	{
	}

	void Update()
	{
		lastLoudness = loudness;
		loudness = lastLoudness * 0.8f + GetAveragedVolume() * sensitivity * 0.2f;
		//loudness = GetAveragedVolume() * sensitivity;
	}

	float GetAveragedVolume()
	{
		float[] data = new float[256];
		float a = 0.0f;
		audio.GetOutputData(data, 0);
		foreach (float s in data)
		{
			a += Mathf.Abs(s);
		}
		return a / 256.0f;
	}
}
