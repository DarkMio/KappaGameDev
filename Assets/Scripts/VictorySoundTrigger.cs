using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class VictorySoundTrigger : MonoBehaviour {
	public AudioClip victory;
	public AudioSource audio;
	public BGM bgm;

	void Start () {
		audio = GetComponent<AudioSource> ();
		bgm = GameObject.Find ("BGM").GetComponent<BGM>();
	}

	void OnTriggerEnter(Collider c) {
		Debug.Log ("Trigger enter");
		if (!audio.isPlaying) {
			audio.PlayOneShot (victory);
			bgm.Pause ();
		}
	}

	void Update() {
		if (!audio.isPlaying && !bgm.isPlaying) {
			bgm.Resume ();
		}
	}
}
