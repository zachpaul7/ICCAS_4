using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;


[System.Serializable]
public class BGMSound
{
    public string name;
    public AudioClip clip;
}

[System.Serializable]
public class SFXSound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource bgmPlayer = null;
    [SerializeField] private AudioClip[] bgm = null;

    [SerializeField] private AudioSource sfxPlayer = null;
    [SerializeField] private AudioSource pSfxPlayer = null;
    [SerializeField] private AudioClip[] sfx = null;
    [SerializeField] private AudioClip[] pSfx = null;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (DataBase.instance.settingInfo.optionToggle[0] == true)
        {
            SetBGMVolume(0.3f);
        }

        if (DataBase.instance.settingInfo.optionToggle[1] == true)
        {
            SetSFXVolume(0.5f);
        }
    }

    #region BGM 설정
    public void ToggleBGM(bool bgmPlay)
    {
        if (bgmPlay)
        {
            SetBGMVolume(0.3f);
        }
        else
        {
            SetBGMVolume(0f);
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmPlayer.volume = volume;
    }

    public void PlayBGM(string bgmName)
    {
        switch (bgmName)
        {
            case "Main":
                bgmPlayer.clip = bgm[0];
                if (bgmPlayer.volume > 0)
                    bgmPlayer.volume = 0.3f;
                bgmPlayer.Play();
                break;

            case "Battle":
                bgmPlayer.clip = bgm[1];
                if (bgmPlayer.volume > 0)
                    bgmPlayer.volume = 0.1f;
                bgmPlayer.Play();
                break;
        }
    }
    #endregion

    #region SFX 설정
    public void ToggleSFX(bool sfxPlay)
    {
        if (sfxPlay)
        {
            SetSFXVolume(0.5f);
        }
        else
        {
            SetSFXVolume(0f);
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxPlayer.volume = volume;
        pSfxPlayer.volume = volume;
    }

    public void PlaySFX(string sfxName)
    {
        switch (sfxName)
        {
            // 효과음
            case "ClickBtn":  // 클릭
                sfxPlayer.PlayOneShot(sfx[0]);
                break;
            case "ConfirmBtn": // 확인
                sfxPlayer.PlayOneShot(sfx[1]);
                break;
            case "CancelBtn":  // 취소, 나가기
                sfxPlayer.PlayOneShot(sfx[2]);
                break;
            case "Reward":  // 보상 획득
                sfxPlayer.PlayOneShot(sfx[3]);
                break;
            case "Clear":  // 클리어 성공
                sfxPlayer.PlayOneShot(sfx[4]);
                break;
            case "Failed":  // 클리어 실패
                sfxPlayer.PlayOneShot(sfx[5]);
                break;

            // Play 음성
            case "1":
                sfxPlayer.PlayOneShot(pSfx[0]);
                break;
            case "2":
                sfxPlayer.PlayOneShot(pSfx[1]);
                break;
            case "3":
                sfxPlayer.PlayOneShot(pSfx[2]);
                break;
            case "4":
                sfxPlayer.PlayOneShot(pSfx[3]);
                break;
            case "5":
                sfxPlayer.PlayOneShot(pSfx[4]);
                break;
            case "6":
                sfxPlayer.PlayOneShot(pSfx[5]);
                break;
            case "7":
                sfxPlayer.PlayOneShot(pSfx[6]);
                break;
            case "8":
                sfxPlayer.PlayOneShot(pSfx[7]);
                break;
            case "9":
                sfxPlayer.PlayOneShot(pSfx[8]);
                break;

            case "M1":
                sfxPlayer.PlayOneShot(pSfx[9]);
                break;
            case "M2":
                sfxPlayer.PlayOneShot(pSfx[10]);
                break;
            case "M3":
                sfxPlayer.PlayOneShot(pSfx[11]);
                break;

            case "S01":
                sfxPlayer.PlayOneShot(pSfx[12]);
                break;
            case "S02":
                sfxPlayer.PlayOneShot(pSfx[13]);
                break;

            case "S11":
                sfxPlayer.PlayOneShot(pSfx[14]);
                break;
            case "S12":
                sfxPlayer.PlayOneShot(pSfx[15]);
                break;
            case "S13":
                sfxPlayer.PlayOneShot(pSfx[16]);
                break;

            case "S21":
                sfxPlayer.PlayOneShot(pSfx[17]);
                break;
            case "S22":
                sfxPlayer.PlayOneShot(pSfx[18]);
                break;
            case "S23":
                sfxPlayer.PlayOneShot(pSfx[19]);
                break;

            case "S31":
                sfxPlayer.PlayOneShot(pSfx[20]);
                break;
            case "S32":
                sfxPlayer.PlayOneShot(pSfx[21]);
                break;

            case "S41":
                sfxPlayer.PlayOneShot(pSfx[22]);
                break;
            case "S42":
                sfxPlayer.PlayOneShot(pSfx[23]);
                break;

            case "S51":
                sfxPlayer.PlayOneShot(pSfx[24]);
                break;
            case "S52":
                sfxPlayer.PlayOneShot(pSfx[25]);
                break;
            case "S53":
                sfxPlayer.PlayOneShot(pSfx[26]);
                break;
        }
    }
    #endregion
}
