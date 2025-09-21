using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public enum SoundType { BGM, SFX }
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // 싱글톤

    [SerializeField] private AudioMixer audioMixer; //오디오 믹서: '여러' 사운드 볼륨 조절용.
    // private float currentBGMVolume, currentSFXVolume; //배경, 효과 사운드 볼륨. >> 오디오믹서를 이용해 개별 조절.
    [SerializeField] private AudioClip[] preloadClips; //미리 불러올 클립.
    [SerializeField] private GameObject temporarySoundPlayerPrefab; //풀링할 프리팹 오브젝트

    private Dictionary<string, AudioClip> clipDictionary; //클립을 담을 딕셔너리.
    private List<TemporarySoundPlayer> activaLoopSounds;


    [SerializeField][Range(0f, 1f)] private float soundEffectVolume;
    [SerializeField][Range(0f, 1f)] private float soundEffectPitchVariance;
    [SerializeField][Range(0f, 1f)] private float musicVolume;

    private AudioSource musicAudioSource;
    // private AudioSource soundSFXSource;
    public AudioClip musicClip;

    public SoundSource soundSourcePrefab;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            clipDictionary = new Dictionary<string, AudioClip>();

            foreach (AudioClip clip in preloadClips) //미리 불러온 클립을 돌아서 딕셔너리에 추가.
            {
                clipDictionary.Add(clip.name, clip);
            }
        }
        else
        {
            Destroy(gameObject);
        }
        musicAudioSource = GetComponent<AudioSource>();
        // soundSFXSource = GetComponent<AudioSource>();

        musicAudioSource.volume = musicVolume;
        musicAudioSource.loop = true;
    }

    private void Start()
    {
        // ChangeBackgroundMusic(musicClip);        

        activaLoopSounds = new List<TemporarySoundPlayer>();
    }

    //BGM 전용
    public void ChangeBackgroundMusic(AudioClip clip)
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }

    public static void PlayClip(AudioClip clip)
    {
        SoundSource obj = Instantiate(Instance.soundSourcePrefab);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, Instance.soundEffectVolume, Instance.soundEffectPitchVariance);
    }

    // public void PlaySoundEffect()
    // {
    //     AudioSource.PlayOneShot(soundSFXSource);
    // }

    //SFX 전용
    private AudioClip GetClip(string clipName)
    {
        if (clipDictionary == null) return null;
        AudioClip clip = clipDictionary[clipName]; //예외처리 필요.

        if (clip == null)
        {
            Debug.LogError(clipName + "이 존재하지 않음.");
        }

        return clip;
    }

    private void AddToList(TemporarySoundPlayer soundPlayer) //사운드 재생 중, 나중에 루프 형태로 재생된 사운드를 제거하기 위해 리스트 저장.
    {
        activaLoopSounds.Add(soundPlayer);
    }

    public void StopLoopSound(string clipName) // 루프 사운드 중 리스트에 있는 오브젝트를 이름으로 찾아 제거함.
    {
        for (int i = activaLoopSounds.Count -1; i >= 0; i--)
        {
            TemporarySoundPlayer player = activaLoopSounds[i];
            if (player.ClipName == clipName)
            {
                activaLoopSounds.RemoveAt(i); //Remove: i를 변환할 수 없음.
                PoolManager.Instance.ReturnObject(player.gameObject); // 사운드 중지 후 풀로 반환.
                return;
            }
        }
        Debug.LogWarning(clipName + "을 찾을 수 없습니다.");
    }

    public void PlaySound2D(string clipName, float delay = 0f, bool isLoop = false, SoundType type = SoundType.SFX)
    {
        //조건#1) 프리팹 오브젝트와 temPorarySoundPlayerPrefab 오브젝트의 이름이 동일해야 한다.
        //조건#2) 오브젝트에 TemporarySoundPlayer 컴포넌트가 있어야 한다.
        GameObject obj = PoolManager.Instance.GetObject(temporarySoundPlayerPrefab, Vector3.zero, Quaternion.identity);
        // obj.SetActive(true);
        TemporarySoundPlayer soundPlayer = obj.GetComponent<TemporarySoundPlayer>();

        if (isLoop)
        {
            AddToList(soundPlayer);
        }

        soundPlayer.InitSound2D(GetClip(clipName)); 
        soundPlayer.Play(audioMixer.FindMatchingGroups(type.ToString())[0], delay, isLoop); // null?

        if (!isLoop) //단발 사운드
        {
            soundPlayer.SetOnFinish(() => PoolManager.Instance.ReturnObject(obj));
        }
    }

    // public void PlaySound3D(string clipName, Transform audioTarget, float delay = 0f, bool isLoop = false, SoundType type = SoundType.SFX, bool attachToTarget = true, float minDistance = 0f, float maxDistance = 50.0f)
    // {
    //     GameObject obj = new GameObject("TemporarySoundPlayer_3D");
    //     obj.transform.localPosition = audioTarget.transform.position;

    //     if (attachToTarget) { obj.transform.parent = audioTarget; }

    //     TemporarySoundPlayer soundPlayer = obj.AddComponent<TemporarySoundPlayer>();

    //     if (isLoop) { AddToList(soundPlayer); } // 루프 시 사운드 저장.

    //     soundPlayer.InitSound3D(GetClip(clipName), minDistance, maxDistance);
    //     soundPlayer.Play(audioMixer.FindMatchingGroups(type.ToString())[0], delay, isLoop);
    // }

    //씬이 로드될 때 모든 사운드 볼륨을 저장된 값으로 초기화시킴.
    public void InitVolumes(float bgm, float sfx)
    {
        SetVolume(SoundType.BGM, bgm);
        SetVolume(SoundType.SFX, sfx);
    }

    public void SetVolume(SoundType type, float value) // 지정한 사운드 그룹의 볼륨을 변경함.
    {
        audioMixer.SetFloat(type.ToString(), value);
    }

    //무작위 사운드를 실행하기 위한 랜덤 값 반환
    //예를 들어 10개 스윙 소리를 같은 이름에 번호만 다르다면,
    //아래 메서드로 랜덤으로 스윙 소리가 나오도록 할 수 있는 로직.
    // public static string Range(int from, int includedTo, bool isStartZero = false)
    // {
    //     if (from > includedTo) // 범위 체크
    //     {
    //         Debug.LogError("'from'이 'included'보다 큽니다.");
    //         return "";
    //     }

    //     int value = UnityEngine.Random.Range(from, includedTo + 1);

    //     if (isStartZero)
    //     {
    //         if (includedTo >= 100)
    //         {
    //             Debug.LogWarning("0을 포함한 세자리는 지원하지 않음");
    //         }

    //         if (value < 10)
    //         {
    //             return "0" + value.ToString();
    //         }
    //     }
    //     return value.ToString();
    // }
}
