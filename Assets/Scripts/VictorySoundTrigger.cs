using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class VictorySoundTrigger : MonoBehaviour {
	public AudioClip victory;
	public AudioSource audio;
	public GameObject BGM;
	public AudioSource bgmSource;

	void Start () {
		audio = GetComponent<AudioSource> ();
		BGM = GameObject.Find ("BGM");
		bgmSource = BGM.GetComponent<AudioSource> ();
	}

	void OnTriggerEnter(Collider c) {
		Debug.Log ("Trigger enter");
		audio.PlayOneShot (victory);
		if (bgmSource.isPlaying) {
			bgmSource.Pause ();
		}
	}

	void OnTriggerExit(Collider c) {
		Debug.Log ("Trigger exit");
		if (!bgmSource.isPlaying) {
			bgmSource.Play ();
		}
	}

	void Update() {
		if (!audio.isPlaying && !bgmSource.isPlaying) {
			bgmSource.Play ();
		}
	}
}
