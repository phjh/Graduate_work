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
	public AudioSource source;
}

public class SoundManager : MonoBehaviour
{
    [Header("Sound Sources")]
	public BGMSource[] bgmList;
	public SFXSource[] sfxList;
    [Header("Main Audio Mixer")]
    public AudioMixer audioMixer;

    private Dictionary<BGMType, AudioSource> bgmDictionary = new Dictionary<BGMType, AudioSource>();
    private Dictionary<SFXType, AudioSource> sfxDictionary = new Dictionary<SFXType, AudioSource>();

    private BGMType bgmSoundMode = BGMType.None;

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
            sfxDictionary.Add(ss.type, ss.source);
        }
    }

	public void ChangeBGMType(BGMType type)
    {
    }
}
