using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Inst;

    public AudioSource OpenWindowSound;
    public AudioSource ButtonSound;
    public AudioSource TitleButtonSound;
    public AudioSource TypingSound;
    public AudioSource BuySound;
    public AudioSource SellSound;
    public AudioSource UseItemSound;
    public AudioSource HealSound;
    public AudioSource LVUpSound;
    public AudioSource ReviveSound;
    public AudioSource CompleteQuestSound;
    public AudioSource MoveClickSound;
    public AudioSource SwordAttackSound;
    public AudioSource BasicSkillSound;
    public AudioSource IceSkillSound;
    public AudioSource ThunderSkillSound;
    public AudioSource RainSound;
    public AudioSource IceRainSound;
    public AudioSource ThunderRainSound;
    public AudioSource FireRainSound;
    public AudioSource[] EnemyAttackSound;
    public AudioSource[] BossSound;
    public AudioSource CannotSound;

    public AudioSource BGM;

    [SerializeField]
    SoundOption soundOption;

    private void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        soundOption.BgmSlider.value = PlayerPrefs.GetFloat("BgmVolume");
        soundOption.SfxSlider.value = PlayerPrefs.GetFloat("SfxVolume");
        BGM.Play();
    }
}
