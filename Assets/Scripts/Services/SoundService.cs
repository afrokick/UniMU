using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundService : MonoBehaviour
{
	private class ClipData
	{
		private static int _idCounter;

		public readonly int Id;
		public readonly AudioClip Clip;
		public readonly AudioSource Source;
		public readonly Sounds Type;
		public readonly bool IsLoop;

		public float ClipLenght { get; private set; }

		public bool IsOnPause { get; private set; }

		private DateTime _startPauseTime;

		public void Pause()
		{
			_startPauseTime = DateTime.Now;
			IsOnPause = true;
			Source.Pause();
		}

		public void UnPause()
		{
			float pauseSeconds = (float)(DateTime.Now - _startPauseTime).TotalSeconds;
			ClipLenght += pauseSeconds;
			Source.Play();
			IsOnPause = false;
		}

		public ClipData(AudioSource source, AudioClip clip, Sounds type, bool isloop)
		{
			Id = ++_idCounter;

			Clip = clip;
			Source = source;
			Type = type;
			IsLoop = isloop;
			ClipLenght = Clip.length;
		}
	}

	public enum Sounds
	{
		MenuTheme,
		GameTheme,
		Click,
		ClickError,
		LevelFinished,
		WordLose,
		WordFinished
	}

    [Inject]
    public IStorageService StorageService { get; private set; }
    [Inject]
    public ICoroutineExecuter CoroutineExecuter { get; private set; }

	public AudioClip MenuTheme, GameTheme;
	public AudioClip Click;
	public AudioClip ClickError;
	public AudioClip LevelFinished;
	public AudioClip WordLose;
	public AudioClip WordFinished;

	public event Action<SoundService, Sounds> SoundFinished = delegate { };
	public event Action<SoundService, Sounds> SoundStopped = delegate { };

	private readonly List<AudioSource> _sources = new List<AudioSource>(16);

	private Dictionary<Sounds, ClipData> _playingSounds = new Dictionary<Sounds, ClipData>(32);

//	private const string AppMuteKey = "app_sound";//mute
	private const string AppSoundKey = "app_sounds";
	private const string AppMusicKey = "app_music";

	private bool IsTheme(Sounds s){
		return s == Sounds.MenuTheme || s == Sounds.GameTheme;
	}

//	public bool IsMute
//	{
//		get
//		{
//			if (!StorageService.Exists(AppMuteKey))
//				return false;
//
//			return !StorageService.GetBool(AppMuteKey);
//		}
//		set
//		{
//			StorageService.Set(AppMuteKey, !value);
//			StorageService.Save();
//
//			CoroutineExecuter.Execute(SetMuteForSound(value));
//		}
//	}

	//	private IEnumerator SetMuteForSound(bool mute)
	//	{
	//		yield return new WaitForSeconds(0.01f);
	//
	//		foreach (var s in _sources)
	//		{
	//			s.mute = mute;
	//		}
	//	}

	public bool IsSound
	{
		get
		{
			if (!StorageService.Exists(AppSoundKey))
				return true;

			return StorageService.GetBool(AppSoundKey);
		}
		set
		{
			StorageService.SetBool(AppSoundKey, value);
			StorageService.Save();

            CoroutineExecuter.Execute(SetSoundState(value));
		}
	}

	public bool IsMusic
	{
		get
		{
			if (!StorageService.Exists(AppMusicKey))
				return true;

			return StorageService.GetBool(AppMusicKey);
		}
		set
		{
			StorageService.SetBool(AppMusicKey, value);
			StorageService.Save();

            CoroutineExecuter.Execute(SetMusicState(value));
		}
	}

	private IEnumerator SetSoundState(bool isOn)
	{
		yield return new WaitForSeconds(0.01f);

		foreach (var pair in _playingSounds) {
			var soundType = pair.Key;

			if(!IsTheme(soundType)){
				pair.Value.Source.mute = !isOn;
			}
		}
	}

	private IEnumerator SetMusicState(bool isOn)
	{
		yield return new WaitForSeconds(0.01f);

		foreach (var pair in _playingSounds) {
			var soundType = pair.Key;

			if(IsTheme(soundType)){
				pair.Value.Source.mute = !isOn;
			}
		}
	}

	public bool IsPlaying(Sounds s)
	{
		return _playingSounds.ContainsKey(s);
	}

	public void Play(Sounds s, bool isLoop = false)
	{
		PlayClip(s, isLoop);
	}

	public void Stop(Sounds s)
	{
		StopClip(s);
	}

	public void StopAll()
	{
		foreach (var sound in  _playingSounds)
		{
			sound.Value.Source.Stop();
			SoundStopped(this, sound.Key);
		}

		_playingSounds.Clear();
	}

	private void PlayClip(Sounds s, bool isLoop)
	{
		AudioClip clip = null;

		switch (s)
		{
		case Sounds.MenuTheme:
			clip = MenuTheme;
			break;
		case Sounds.GameTheme:
			clip = GameTheme;
			break;
		case Sounds.Click:
			clip = Click;
			break;
		case Sounds.ClickError:
			clip = ClickError;
			break;
		case Sounds.LevelFinished:
			clip = LevelFinished;
			break;
		case Sounds.WordLose:
			clip = WordLose;
			break;
		case Sounds.WordFinished:
			clip = WordFinished;
			break;
		default:
			break;
		}

		SetPlaying(s, clip, isLoop);
	}

	private void SetPlaying(Sounds s, AudioClip clip, bool isLoop)
	{
		var source = GetSource();

		if(IsTheme(s))
			source.mute = !IsMusic;
		else
			source.mute = !IsSound;

		source.clip = clip;
	    source.loop = isLoop;

		source.Play();

		var cd = new ClipData(source, clip, s, isLoop);
		_playingSounds[s] = cd;
	}

	private void StopClip(Sounds s)
	{
		if (_playingSounds.ContainsKey(s))
		{
			var clipData = _playingSounds[s];

			clipData.Source.Stop();

			_playingSounds.Remove(s);

			SoundStopped(this, s);
		}
	}

	public void PauseClip(Sounds s)
	{
		if (_playingSounds.ContainsKey(s))
		{
			var clipData = _playingSounds[s];

			clipData.Pause();
		}
	}

	public void UnPauseClip(Sounds s)
	{
		if (_playingSounds.ContainsKey(s))
		{
			var clipData = _playingSounds[s];

			clipData.UnPause();
		}
	}

	private AudioSource GetSource()
	{
		foreach (var s in _sources)
		{
			if (!s.isPlaying)
			{
				bool isFounded = false;
				foreach (var ss in _playingSounds.Values)
				{
					if (ss.Source == s)
					{
						isFounded = true;
						break;
					}
				}

				if (isFounded)
					continue;

				return s;
			}
		}

		var source = gameObject.AddComponent<AudioSource>();
		_sources.Add(source);

		return source;
	}
}
