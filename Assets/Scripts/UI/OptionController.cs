using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider soundBGMSlider;
    [SerializeField] private Slider soundSFXSlider;
    [SerializeField] private GameObject OptionUI;

    void Start()
    {
        float bgmVolumeDb;
        float sfxVolumeDb;
        if (audioMixer.GetFloat("BGM", out bgmVolumeDb) && audioMixer.GetFloat("SFX", out sfxVolumeDb))
        {
            soundBGMSlider.value = Mathf.InverseLerp(-80f, 0f, bgmVolumeDb);
            soundSFXSlider.value = Mathf.InverseLerp(-80f, 0f, sfxVolumeDb);
        }
        
    }

    //옵션창 볼륨 조절
    public void SliderModify_BGM_Volume()
    {
        float dB = Mathf.Lerp(-80f, 0f, soundBGMSlider.value);
        SoundManager.Instance.SetVolume(SoundType.BGM, dB);
    }

    public void SliderModify_SFX_Volume()
    {
        float dB = Mathf.Lerp(-80f, 0f, soundSFXSlider.value);
        SoundManager.Instance.SetVolume(SoundType.SFX, dB);
    }

    // UI 관리
    public void ButtonCloseOptionUI()
    {
        OptionUI.SetActive(!OptionUI.activeSelf);
    }
}
