using UnityEngine;
using System.Collections;

public class BGM : MonoBehaviour {
	public IntroloopAudio myIntroloopAudio;
	public IntroloopPlayer player;
	public bool isPlaying = false;

	public void Play() {
		player.PlayFade (myIntroloopAudio);
		isPlaying = true;
	}

	public void Pause() {
		player.PauseFade ();
		isPlaying = false;
	}

	public void Resume() {
		player.ResumeFade ();
		isPlaying = true;
	}

	private void Start() {
		player = IntroloopPlayer.Instance;
		Play ();
	}
}
