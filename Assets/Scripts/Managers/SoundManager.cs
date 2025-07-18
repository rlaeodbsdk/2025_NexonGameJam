using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource audioSource;       // BGM용
    public AudioSource onlyFireSource;    // 루프 사운드 (예: 화염방사기)
    public AudioSource SFXSource;         // 일반 효과음용
    public List<AudioClip> clipList;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 🎵 배경음악 재생
    public void AudioPlay(string audioName)
    {
        audioSource.clip = clipList.Find(x => x.name == audioName);

        if (audioSource.clip == null)
        {
            Debug.LogWarning($"BGM '{audioName}' not found in clipList!");
            return;
        }

        audioSource.loop = true;
        audioSource.Play();
    }

    // ⏹️ 배경음악 정지
    public void AudioStop()
    {
        audioSource.Stop();
    }

    // 🔊 일반 효과음 재생
    public void SFXPlay(string SFXName)
    {
        AudioClip clip = clipList.Find(x => x.name == SFXName);
        if (clip == null)
        {
            Debug.LogWarning($"SFX '{SFXName}' not found in clipList!");
            return;
        }   

        float volume = 1.0f;
        if (SFXName == "heal")
        {
            volume = 3.0f;
        }

        SFXSource.PlayOneShot(clip, volume);
    }

    // 🔁 루프 효과음 재생
    public void SFXPlay(string SFXName, bool loop)
    {
        if (!loop)
        {
            SFXPlay(SFXName);
            return;
        }

        AudioClip clip = clipList.Find(x => x.name == SFXName);
        if (clip == null)
        {
            Debug.LogWarning($"Looping SFX '{SFXName}' not found in clipList!");
            return;
        }

        if (onlyFireSource.isPlaying && onlyFireSource.clip == clip)
            return; // 이미 재생 중이면 중복 방지

        onlyFireSource.clip = clip;
        onlyFireSource.loop = true;
        onlyFireSource.Play();
    }

    // 🔇 루프 사운드 중단
    public void SFXStop(string SFXName)
    {
        if (onlyFireSource.isPlaying && onlyFireSource.clip != null && onlyFireSource.clip.name == SFXName)
        {
            onlyFireSource.Stop();
            onlyFireSource.clip = null;
        }
    }

    // 🔊 볼륨 조절
    public void ChangeVolume(float value)
    {
        audioSource.volume = value;
        SFXSource.volume = value;
        onlyFireSource.volume = value;
    }
}
