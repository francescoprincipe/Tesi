using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0f, 1f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;

    public bool loop = false;

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play()
    {
        source.volume = volume * (1 + UnityEngine.Random.Range(-randomVolume / 2f, randomVolume / 2f));
        source.pitch = pitch * (1 + UnityEngine.Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }

    public void PlayBackground()
    {
        if (source != null && !source.isPlaying )
        {
            source.volume = volume * (1 + UnityEngine.Random.Range(-randomVolume / 2f, randomVolume / 2f));
            source.pitch = pitch * (1 + UnityEngine.Random.Range(-randomPitch / 2f, randomPitch / 2f));
            source.Play();
        }
    }

    public void Stop()
    {
        source.Stop();
    }

    public void Pause()
    {
        source.Pause();
    }

    public void UnPause()
    {
        source.UnPause();
    }

    public void SetVolume(float newVolume)
    { 
        if(source != null)
        source.volume = newVolume * (1 + UnityEngine.Random.Range(-randomVolume / 2f, randomVolume / 2f));
    }

    public bool IsPlaying()
    {
        return source.isPlaying;
    }
}

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    [SerializeField]
    Sound[] sounds;

    [SerializeField]
    private string menuSoundName;

    [SerializeField]
    private string gameSoundName;

    public float masterVolume = 0.5f;


    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(gameObject);
                //Debug.LogError("More than one AudioManager in the scene.");
            }
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }
        Debug.Log("PlayMenu");
        PlayBackgroundSound(menuSoundName);
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }

        // no sound with _name
        Debug.Log("AudioManager: Sound not found in list: " + _name);
    }

    public void PlayBackgroundSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].PlayBackground();
                return;
            }
        }

        // no sound with _name
        Debug.Log("AudioManager: Sound not found in list: " + _name);
    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }

        // no sound with _name
        Debug.Log("AudioManager: Sound not found in list: " + _name);
    }

    public void PauseSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Pause();
                return;
            }
        }

        // no sound with _name
        Debug.Log("AudioManager: Sound not found in list: " + _name);
    }

    public void UnPauseSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].UnPause();
                return;
            }
        }

        // no sound with _name
        Debug.Log("AudioManager: Sound not found in list: " + _name);
    }

    public void SetSoundVolume(string _name, float newVolume)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].SetVolume(newVolume);
                return;
            }
        }

        // no sound with _name
        Debug.Log("AudioManager: Sound not found in list: " + _name);
    }
    public void SetMasterVolume(float newVolume)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].SetVolume(newVolume);
        }

        masterVolume = newVolume;
        return;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

            if (scene.name == "Menu")
            {
            try
            {
                StopSound(gameSoundName);
            }
            catch (Exception e)
            {
                Debug.Log("Nessun suono da stoppare trovato");
            }
                PlayBackgroundSound(menuSoundName);
                SetSoundVolume(menuSoundName, masterVolume);
            }

            if (scene.name == "Game")
            {
            try
            {
                StopSound(menuSoundName);
            }
            catch (Exception e)
            {
                Debug.Log("Nessun suono da stoppare trovato");
            }
                PlayBackgroundSound(gameSoundName);
                SetSoundVolume(gameSoundName, masterVolume);
            }
        

    }
}


