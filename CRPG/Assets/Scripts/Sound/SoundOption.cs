using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundOption : MonoBehaviour
{
    public AudioMixer audioMixer;

    [SerializeField]
    Sprite BgmON;
    [SerializeField]
    Sprite BgmOff;
    [SerializeField]
    Sprite SfxOn;
    [SerializeField]
    Sprite SfxOff;

    public Image BgmIcon;
    public Image SfxIcon;

    public Slider BgmSlider;
    public Slider SfxSlider;

    private void Update()
    {
        if(BgmSlider.value > 0.0001f)
        {
            BgmIcon.sprite = BgmON;
        }
        else
        {
            BgmIcon.sprite = BgmOff;
        }
        if(SfxSlider.value > 0.0001f)
        {
            SfxIcon.sprite = SfxOn;
        }
        else
        {
            SfxIcon.sprite = SfxOff;
        }
    }

    public void SetBgmVolume()
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(BgmSlider.value) * 20);
    }

    public void SetSfxVolume()
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(SfxSlider.value) * 20);
    }

    public void CloseWindow()
    {
        PlayerPrefs.SetFloat("BgmVolume", BgmSlider.value);
        BgmSlider.value = PlayerPrefs.GetFloat("BgmVolume");
        PlayerPrefs.SetFloat("SfxVolume", SfxSlider.value);
        SfxSlider.value = PlayerPrefs.GetFloat("SfxVolume");
    }
}
