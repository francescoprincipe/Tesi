using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;
    public void Awake()
    {
        slider = this.GetComponent<Slider>();
    }

    private void OnEnable()
    {
        slider.value = AudioManager.Instance.masterVolume;
    }
    public void SetVolume(float volume)
    {
        AudioManager.Instance.SetMasterVolume(volume);
    }
}
