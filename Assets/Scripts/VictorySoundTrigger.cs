using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class VictorySoundTrigger : MonoBehaviour {
	public AudioClip victory;
	public AudioSource audioSource;
	public BGM bgm;

	void Start () {
		audioSource = GetComponent<AudioSource> ();
		bgm = GameObject.Find ("BGM").GetComponent<BGM>();
	}

	void OnTriggerEnter(Collider c) {
		Debug.Log ("Trigger enter");
		if (!audioSource.isPlaying) {
			audioSource.PlayOneShot (victory);
			bgm.Pause ();
		}
	}

	void Update() {
		if (!audioSource.isPlaying && !bgm.isPlaying) {
			bgm.Resume ();
		}
	}
}
