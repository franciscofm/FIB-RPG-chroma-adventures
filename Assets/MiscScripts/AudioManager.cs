using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	//Add audio clips if needed
	public AudioClip NavMusic;
	public AudioClip CombatMusic;

	//Add audio sources, one for music, one for sounds
	public AudioSource MusicSource;
	public AudioSource SoundSource;

	void PlayNavMusic () {
		if (MusicSource.isPlaying)
			MusicSource.Stop ();
		MusicSource.clip = NavMusic;
		MusicSource.loop = true;
		MusicSource.Play ();
	}

	void PlayCombatMusic () {
		if (MusicSource.isPlaying)
			MusicSource.Stop ();
		MusicSource.clip = CombatMusic;
		MusicSource.loop = true;
		MusicSource.Play ();
	}

	void PlaySound (AudioClip clip) {
		if (SoundSource.isPlaying)
			SoundSource.Stop ();			
		SoundSource.clip = clip;
		SoundSource.Play ();
	}
}
