using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BgmSound
{
    Start
}

public enum Sfx
{
    Clear,
    Move,
    Success
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> BgmList;
    [SerializeField] List<AudioClip> SfxList;
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource sfx;

    [SerializeField] List<AudioSource> sfxPool;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        BgmList = new List<AudioClip>()
        {
            Resources.Load<AudioClip>("Sound/Start")
        };

        SfxList = new List<AudioClip>()
        {
            Resources.Load<AudioClip>("Sound/Clear"),
            Resources.Load<AudioClip>("Sound/Move"),
            Resources.Load<AudioClip>("Sound/Success")
        };

        sfxPool = new List<AudioSource>();
    }

    AudioSource GetSFX()
    {
        AudioSource select = null;

        foreach (AudioSource audioSource in sfxPool)
        {
            if (audioSource != null && audioSource.isPlaying == false)
            {
                select = audioSource;
                break;
            }
        }

        if (select == null)
        {
            select = Instantiate(sfx, transform);
            sfxPool.Add(select);
        }
        return select;
    }

    public void PlayBGM(BgmSound type)
    {
        bgm.clip = BgmList[(int)type];
        bgm.Play();
    }

    public void StopBGM()
    {
        bgm.Stop();
    }

    public void PlaySFX(Sfx type)
    {
        AudioSource sfx = GetSFX();
        sfx.clip = SfxList[(int)type];
        sfx.Play();
    }

    public void AllStopSFX()
    {
        foreach (AudioSource sfx in sfxPool)
        {
            sfx.Stop();
        }
    }

    public void BgmVolumeSettimg(float volume)
    {
        bgm.volume = volume;
    }

    public void SfxVolumeSettimg(float volume)
    {
        sfx.volume = volume;

        foreach (AudioSource sfx in sfxPool)
        {
            sfx.volume = volume;
        }
    }

    public void MuteOn()
    {
        bgm.mute = true;
        sfx.mute = true;
        foreach (AudioSource sfx in sfxPool)
        {
            sfx.mute = true;
        }
    }

    public void MuteOff()
    {
        bgm.mute = false;
        sfx.mute = false;
        foreach (AudioSource sfx in sfxPool)
        {
            sfx.mute = false;
        }
    }

    public AudioSource GetBgm()
    {
        return bgm;
    }
}