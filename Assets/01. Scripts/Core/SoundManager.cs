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
	public AudioSource source;
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
    public AudioMixer audioMixer;
    [Header("Audio Objects")]
    public GameObject SFXPlayerPrefab;

    private Dictionary<BGMType, AudioSource> bgmDictionary = new Dictionary<BGMType, AudioSource>();
    private Dictionary<SFXType, AudioClip> sfxDictionary = new Dictionary<SFXType, AudioClip>();

    private BGMType bgmSoundMode = BGMType.None;

	public override void InitManager()
	{
		base.InitManager();

		SetUpDicitonarys();
	}

	private void SetUpDicitonarys()
    {
        if(bgmDictionary.Count > 0) { bgmDictionary.Clear(); }
        foreach (BGMSource bs in bgmList)
        {
            bgmDictionary.Add(bs.type, bs.source);
        }
        
        if(sfxDictionary.Count > 0) {  sfxDictionary.Clear(); }
        foreach (SFXSource ss in sfxList)
        {
            sfxDictionary.Add(ss.type, ss.clip);
        }
    }

	public void ChangeBGMType(BGMType type)
    {
        if(bgmDictionary.ContainsKey(type) == false)
        {
            Logger.LogWarning($"{type} BGM clip has not exist");
            return;
        }

        if(bgmSoundMode != BGMType.None)
        {
            bgmDictionary[type].Stop();
        }

        bgmSoundMode = type;

        bgmDictionary[bgmSoundMode].Play();
    }

    public void PlaySFX(SFXType type)
    {
		if (sfxDictionary.ContainsKey(type) == false)
		{
			Debug.LogError($"{type} type SFX clip has not exist.");

			return;
		}

        GameObject AudioPlayer = Instantiate(SFXPlayerPrefab);
        AudioPlayer.transform.position = transform.position;
        if (AudioPlayer.TryGetComponent(out AudioSource AS))
        {
            AS.clip = sfxDictionary[type];
            if(AS.outputAudioMixerGroup == null)
            {
                if(audioMixer.FindMatchingGroups("SFX")[0] == true) AS.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
			}
            AS.Play();
			//Destroy Player In Calculation time
			Destroy(AudioPlayer, sfxDictionary[type].length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
        }
        else
        {
            Logger.LogError($"{SFXPlayerPrefab.name}'s AudioSource is Null");
            Destroy(AudioPlayer);
        }
	}

}
