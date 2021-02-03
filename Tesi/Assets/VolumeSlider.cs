using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSlider : MonoBehaviour
{
    public void SetVolume(float volume)
    {
        AudioManager.Instance.SetMasterVolume(volume);
    }
}
