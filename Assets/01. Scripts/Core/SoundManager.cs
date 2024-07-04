using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum BGMType
{
    None,
    Title,
    Lobby,
    InGame,
    BossFight,
    BossClear,
    Loading
}

public enum SFXType
{
    None,
    Player_Attack,
    Player_Hit,
    Player_Die,
    Enemy_Attack,
    Enemy_Hit,
    Enemy_Die,
    Portal_Open,
    Wall_Broken,
    Ore_Broken,
    Ore_Collect,
    UI_Button_Push,
}

[System.Serializable]
public struct BGMSource
{
	public BGMType type;
	public int ID;
	public AudioClip clip;
}

[System.Serializable]
public struct SFXSource
{
	public SFXType type;
	public int ID;
	public string objName;
	public AudioClip clip;
}

public class SoundManager : ManagerBase<SoundManager>
{
    [Header("Sound Sources")]
	public BGMSource[] bgmList;
	public SFXSource[] sfxList;
    [Header("Main Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [Header("Audio Objects")]
	public AudioSource BGMPlayer;
    public GameObject SFXPlayerPrefab;

    private Dictionary<BGMType, AudioClip> bgmDictionary = new Dictionary<BGMType, AudioClip>();
    private Dictionary<SFXType, AudioClip> sfxDictionary = new Dictionary<SFXType, AudioClip>();

    private BGMType bgmSoundMode = BGMType.None;

	private void OnEnable()
	{
		if (audioMixer == null)
		{
			AudioMixer resource = Resources.Load<AudioMixer>("MainAudioMixer");
			audioMixer = resource;
		}
	}

	public override void InitManager()
	{
		base.InitManager();
		
		SetPlayers();
		SetUpDicitonarys();
	}

	private void SetPlayers() //Set DontDestroyOnLoad BGMPlayer
	{
		if(BGMPlayer == null) //if BGM Player is null, Make New BGM Player
		{
			GameObject player = new GameObject( name = "BGM_Player");

			BGMPlayer = player.AddComponent<AudioSource>();
		}
		
		BGMPlayer.transform.parent = transform;
		DontDestroyOnLoad(BGMPlayer);

		if(SFXPlayerPrefab == null)
		{
			SFXPlayerPrefab = Resources.Load<GameObject>("Prefabs/SFX_Player");
		}
		
		Logger.Log($"Set SFXPlayer : {SFXPlayerPrefab != null}");
	}

	private void SetUpDicitonarys() // Set Up Dictionarys to List Values
    {
        if(bgmDictionary.Count > 0) { bgmDictionary.Clear(); }
        foreach (BGMSource bs in bgmList)
        {
            bgmDictionary.Add(bs.type, bs.clip);
        }
        
        if(sfxDictionary.Count > 0) {  sfxDictionary.Clear(); }
        foreach (SFXSource ss in sfxList)
        {
            sfxDictionary.Add(ss.type, ss.clip);
        }
    }

	public void ChangeBGMType(BGMType type, float pitch = 1.0f)
    {
        if(bgmDictionary.ContainsKey(type) == false)
        {
            Logger.LogWarning($"{type} BGM clip has not exist");
            return;
        }


        if(bgmSoundMode != BGMType.None)
        {
			BGMPlayer.Stop();
			BGMPlayer.clip = bgmDictionary[type];
			BGMPlayer.pitch = pitch;
        }

        bgmSoundMode = type;

		BGMPlayer.loop = true;

        BGMPlayer.Play();
    }

	public void StopBGM(bool isPause = false)
	{
		if(isPause == true)
		{
			BGMPlayer.Pause();
			return;
		}
		else if(isPause == false)
		{
			BGMPlayer.Stop();
			return;
		}
	}

	public void PlaySFX(SFXType type, float volume = 0.5f, float pitch = 1.0f)
	{
		if (!sfxDictionary.ContainsKey(type))
		{
			Logger.LogError($"{type} type SFX clip does not exist.");
			return;
		}

		GameObject audioPlayer = Instantiate(SFXPlayerPrefab);
		audioPlayer.transform.position = transform.position;

		if (audioPlayer.TryGetComponent(out AudioSource audioSource))
		{
			audioSource.clip = sfxDictionary[type];
			audioSource.volume = volume;
			audioSource.pitch = pitch;

			if (audioSource.outputAudioMixerGroup == null)
			{
				var sfxGroups = audioMixer.FindMatchingGroups("SFX");
				if (sfxGroups.Length > 0)
				{
					audioSource.outputAudioMixerGroup = sfxGroups[0];
				}
			}

			audioSource.Play();

			// Calculate destroy time
			float destroyTime = sfxDictionary[type].length * Mathf.Max(0.01f, Time.timeScale);
			Destroy(audioPlayer, destroyTime);
		}
		else
		{
			Logger.LogError($"{SFXPlayerPrefab.name} does not have an AudioSource component.");
			Destroy(audioPlayer);
		}
	}

	#region Set Volume

	public void SetMasterVolume(float volume)
	{
		audioMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20f);
	}

	public void SetSoundFXXVolume(float volume)
	{

		audioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20f);
	}

	public void SetBGMVolume(float volume)
	{

		audioMixer.SetFloat("bgmVolume", Mathf.Log10(volume) * 20f);
	}

	#endregion

}
