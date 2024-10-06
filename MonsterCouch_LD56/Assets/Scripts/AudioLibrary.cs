using System.Collections.Generic;
using UnityEngine;

public class AudioLibrary : MonoBehaviour
{
	public static AudioLibrary Instance { get; private set; }

	[SerializeField] private AudioClip _backgroundMusic;
	[SerializeField] private AudioClip _couchEatsSock;
	[SerializeField] private AudioClip _couchMoving;
	[SerializeField] private AudioClip _socksMatched;

	private Dictionary<AudioClipType, AudioClip> _audioClips;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
			InitializeAudioClips();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void InitializeAudioClips()
	{
		_audioClips = new Dictionary<AudioClipType, AudioClip>
		{
			{ AudioClipType.BackgroundMusic, _backgroundMusic },
			{ AudioClipType.CouchEatsSock, _couchEatsSock },
			{ AudioClipType.CouchMoving, _couchMoving },
			{ AudioClipType.SocksMatched, _socksMatched }
		};
	}

	public AudioClip GetAudioClip(AudioClipType type)
	{
		return _audioClips.TryGetValue(type, out var clip) ? clip : null;
	}
}

public enum AudioClipType
{
	BackgroundMusic,
    CouchEatsSock,
	CouchMoving,
	SocksMatched
}