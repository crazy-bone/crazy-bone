using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource sfxSound;
    public AudioSource bgmSound;

    void Start()
    {
        // 옵션 로드
        ConfigManager.LoadConfigData();
        // 볼륨 적용
        sfxSound.volume = ConfigManager.currentConfig.sfxVolume;
        bgmSound.volume = ConfigManager.currentConfig.bgmVolume;
    }
}
