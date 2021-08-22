using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionModal : MonoBehaviour
{
    public Slider sfxVolumeSlider;
    public Slider bgmVolumeSlider;

    void Awake()
    {
        sfxVolumeSlider.value = ConfigManager.currentConfig.sfxVolume;
        bgmVolumeSlider.value = ConfigManager.currentConfig.bgmVolume;
    }

    public void OnApplyButtonClicked()
    {
        gameObject.SetActive(false);
    }

    public void OnCancelButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
