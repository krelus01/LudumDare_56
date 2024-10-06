using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager Instance { get; private set; }

	[SerializeField] private AudioSource _musicSource;
	[SerializeField] private AudioSource _effectsSource;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void PlayMusic(AudioClipType clipType)
	{
		AudioClip clip = AudioLibrary.Instance.GetAudioClip(clipType);
		if (clip != null)
		{
			_musicSource.clip = clip;
			_musicSource.Play();
		}
	}

	public void StopMusic()
	{
		_musicSource.Stop();
	}

	public void PlayEffect(AudioClipType clipType)
	{
		AudioClip clip = AudioLibrary.Instance.GetAudioClip(clipType);
		if (clip != null)
		{
			_effectsSource.PlayOneShot(clip);
		}
	}

	public void StopEffect()
	{
		_effectsSource.Stop();
	}
}