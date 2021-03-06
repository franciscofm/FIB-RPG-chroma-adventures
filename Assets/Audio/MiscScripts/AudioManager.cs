using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	//Add audio clips if needed
	public AudioClip NavMusic;
	public AudioClip CombatMusic;

	public AudioClip combatEnter;
	public AudioClip victory;
	public AudioClip punch;
	public AudioClip whoosh;

	//Add audio sources, one for music, one for sounds
	public AudioSource MusicSource;
	public AudioSource SoundSource;

	public void MusicStop() {
			MusicSource.Stop ();
	}

	public void SoundStop() {
			SoundSource.Stop ();
	}

	public void PlayNavMusic () {
		MusicSource.Stop ();
		MusicSource.clip = NavMusic;
		MusicSource.loop = true;
		MusicSource.Play ();
	}

	public void PlayCombatMusic () {
		MusicSource.Stop ();
		PlayCombatEnter ();
		MusicSource.clip = CombatMusic;
		MusicSource.loop = true;
		MusicSource.Play ();
	}

	public void PlayCombatEnter() {
		SoundSource.Stop ();			
		SoundSource.clip = combatEnter;
		SoundSource.Play ();
	}

	public void PlayVictory() {
		SoundSource.Stop ();			
		SoundSource.clip = victory;
		SoundSource.Play ();
	}

	public void PlayPunch() {
		SoundSource.Stop ();			
		SoundSource.clip = punch;
		SoundSource.Play ();
	}

	public void PlayWhoosh() {
		SoundSource.Stop ();			
		SoundSource.clip = whoosh;
		SoundSource.Play ();
	}
}
