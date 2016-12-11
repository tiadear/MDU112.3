using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	public static SoundController Instance { get; private set; }

	public AudioSource SFX;
	public AudioSource BackgroundMusic;
	public AudioSource UI;

	public AudioClip[] HitWall;
	public AudioClip[] Shoot;
	public AudioClip[] EatenBySpider;
	public AudioClip[] PowerUp;

	void Start() {
		Instance = this;
	}

	private void PlaySound(AudioSource source, AudioClip clip) {
		source.PlayOneShot (clip);
	}

	public static void OnHitWall() {
		AudioClip clip = Instance.HitWall[0];
		Instance.PlaySound (Instance.SFX, clip);
	}

	public static void OnShoot() {
		AudioClip clip = Instance.Shoot[0];
		Instance.PlaySound (Instance.SFX, clip);
	}

	public static void OnEatenBySpider() {
		AudioClip clip = Instance.EatenBySpider[0];
		Instance.PlaySound (Instance.SFX, clip);
	}

	public static void OnPowerUp() {
		AudioClip clip = Instance.PowerUp[0];
		Instance.PlaySound (Instance.SFX, clip);
	}
}
