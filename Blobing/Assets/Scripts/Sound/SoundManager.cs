using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public AudioSource[] audioSources;
	public AllSoundClips clips;
	public AudioSource musicSource;

    public static SoundManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }


    public void SetMusic()
	{
		musicSource.clip = clips.gameMusic;
		musicSource.Play();
	}

	public void PlaySound (AudioClip clip, float volume, bool randomPitch, float minPitch, float maxPitch) {
		
		AudioSource chosenAudiosource = null;
		foreach (AudioSource source in audioSources) {
			if (!source.isPlaying) {
				chosenAudiosource = source;
				break;
			}
		}

		if (chosenAudiosource == null) {
			return;
		}

		chosenAudiosource.pitch = Random.Range (minPitch, maxPitch);
		
		chosenAudiosource.PlayOneShot (clip, volume);

	}

	public void PlaySound (AudioClip clip, float volume, float pitch) {
		
		AudioSource chosenAudiosource = null;
		foreach (AudioSource source in audioSources) {
			if (!source.isPlaying) {
				chosenAudiosource = source;
				break;
			}
		}

		if (chosenAudiosource == null) {
			return;
		}

		chosenAudiosource.pitch = pitch;
		chosenAudiosource.PlayOneShot (clip, volume);

	}

	public void PlaySoundAtPos (AudioClip clip, float volume, Vector3 pos) {
		AudioSource.PlayClipAtPoint(clip, pos, volume);
	}
}


