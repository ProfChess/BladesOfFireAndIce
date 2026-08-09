using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private MusicTrack[] musicTracks;
    [SerializeField] private SoundSettings soundSettings;

    [Header("SFX")]
    [SerializeField] private AudioSource MainSFXSource;
    [SerializeField] private SoundEffect[] soundEffects;
    [SerializeField] private GameObject ExtraAudioSourcesParent;
    private AudioSource[] sfxPool;

    private Dictionary<SFX, SoundEffect> effectMap = new();
    private Dictionary<BGM, AudioClip> musicMap = new();
    private BGM CurrentMusic;

    [Header("Sliders")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;

    private void Awake()
    {
        //Create Dictionaries for SFX and BGM
        foreach (var sound in soundEffects)
        {
            if (!effectMap.ContainsKey(sound.SoundID))
            {
                effectMap.Add(sound.SoundID, sound);
            }
        }
        foreach (var music in musicTracks)
        {
            if (!musicMap.ContainsKey(music.MusicID))
            {
                musicMap.Add(music.MusicID, music.Clip);
            }
        }

        sfxPool = ExtraAudioSourcesParent.GetComponentsInChildren<AudioSource>();


    }
    private void Start()
    {
        //InitializeAudioValues
        InitializeAudio();
    }
    public void PlaySoundEffect(SFX effect)
    {
        if (!effectMap.TryGetValue(effect, out SoundEffect effectData)) return;

        if (!effectData.randomizePitch)
        {
            MainSFXSource.PlayOneShot(effectData.Clip, effectData.Volume);
            return;
        }

        AudioSource source = GetAvailableAudioSource();
        if (source != null)
        {
            source.pitch = Random.Range(effectData.MinPitch, effectData.MaxPitch);
            source.PlayOneShot(effectData.Clip, effectData.Volume);
        }
        else
        {
            MainSFXSource.PlayOneShot(effectData.Clip, effectData.Volume);
        }
    }
    public void PlayMusic(BGM Music)
    {
        if (CurrentMusic != Music)
        {
            MusicSource.Stop();
            MusicSource.clip = musicMap[Music];
            CurrentMusic = Music;
            MusicSource.Play();
        }

    }
    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in sfxPool)
        {
            if (!source.isPlaying) { return source; }
        }
        return null;
    }


    public void PlayMusicOnSceneChange(Scene scene, LoadSceneMode mode)
    {
        SceneBGSoundChoice BGMChoice = FindObjectOfType<SceneBGSoundChoice>();

        if (BGMChoice != null)
        {
            PlayMusic(BGMChoice.MusicChoice);
        }
    }

    //Music

    private float ConvertAudioValue(float value)
    {
        return Mathf.Log10(value) * 20f;
    }
    public void ChangeAudio_SFX()
    {
        mainMixer.SetFloat("SFXVolume", ConvertAudioValue(SFXSlider.value));
    }
    public void ChangeAudio_BGM()
    {
        mainMixer.SetFloat("MusicVolume", ConvertAudioValue(BGMSlider.value));
    }
    public void ChangeAudio_Master()
    {
        mainMixer.SetFloat("MasterVolume", ConvertAudioValue(MasterSlider.value));
    }
    public void InitializeAudio()
    {
        ChangeAudio_Master();
        ChangeAudio_BGM();
        ChangeAudio_SFX();
    }
}
[System.Serializable]
public class MusicTrack
{
    public BGM MusicID;
    public AudioClip Clip;
}

[System.Serializable]
public class SoundEffect
{
    public SFX SoundID;
    public AudioClip Clip;
    public bool randomizePitch = false;
    public float MinPitch = 0.9f;
    public float MaxPitch = 1.1f;
    public float Volume = 1f;
}
public enum BGM { None, Menu, Dungeon, Hub, Paused, FireBoss, IceBoss, ThirdBoss, FinalBoss}
public enum SFX { PlayerDamaged = 0, PlayerSwordSwingFire1 = 1, PlayerSwordSwingFire2 = 2, PlayerSwordSwingIce1 = 3, 
                  PlayerSwordSwingIce2 = 4, PlayerDeath = 5, PlayerSwitchStance = 6, PlayerRoll = 7, 
                  PlayerParrySuccess = 8, PlayerParryMiss = 9, PlayerHitBlocked = 10, PlayerInteract = 11, 
                  PlayerBlockBroken = 12, PlayerCastIce = 13, PlayerCastFire = 14,

                  MenuButtonHover = 20, MenuButtonPressed = 21, GamePaused = 22, GameUnpaused = 23,
                  MapOpened = 24, MapClosed = 25, InventoryOpened = 26, InventoryClosed = 27, 
                  InventoryObjHovered = 28, InventoryObjSelected = 29, PickupEquipped = 30, ShopItemPurchased = 31, 
                  InvalidAction = 32, 

                  EnemyHurtHeavy = 40, EnemyHurtLight = 41, EnemyProjArrow = 42, EnemyProjMagic = 43, 

                  PuzzleCompleted = 51, DoorDisappear = 52, GoldCollectedSmall = 53, 
                  GoldCollectedMed = 54, GoldCollectedLarge = 55, EssenceCollected = 56, SkillPointPlaced = 57, 
                  BlessingEquipped = 58, DecorationBreakPot = 59, DecorationBreakBox = 60, ChestLocked = 61, 
                  ChestUnlocked = 62, ChestOpened = 63,

                  BossPhaseChanged = 70, BossDefeated = 71, Boss_FireEle_Attack1 = 72, Boss_FireEle_Attack2 = 73,
                  Boss_FireEle_Attack3 = 74, Boss_FireEle_Attack4 = 75,
                  Boss_IceEle_Attack1 = 80, Boss_IceEle_Attack2 = 81, Boss_IceEle_Attack3 = 82, Boss_IceEle_Attack4 = 83,
                  Boss_Wizard_Attack1 = 90, Boss_Wizard_Attack2 = 91, Boss_Wizard_Attack3 = 92, Boss_Wizard_Attack4 = 93,
}
