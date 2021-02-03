using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class OptionsManager : MonoBehaviour
{

    private static OptionsManager _instance;
    public static OptionsManager Instance { get { return _instance; } }

    public int characterSelected = 0;
    private GameObject[] characters;
    public int numberOfCharacters;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
           _instance = this;
        }

    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Menu")
        {
            GetCharactersHolder();
            foreach(GameObject character in characters)
            {
                character.SetActive(false);
            }
            characters[characterSelected].SetActive(true);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void GoNextCharacter()
    {
        characters[characterSelected].SetActive(false);
        characterSelected++;
        if (characterSelected >= characters.Length)
        {
            characterSelected = 0;
        }
        characters[characterSelected].SetActive(true);
    }

    public void GoPreviousCharacter()
    {
        characters[characterSelected].SetActive(false);
        characterSelected--;
        if (characterSelected < 0)
        {
            characterSelected = characters.Length - 1;
        }
        characters[characterSelected].SetActive(true);
    }

    public void GetCharactersHolder()
    {
        GameObject charHolder = FindInActiveObjectByTag("CharactersHolder");
        //Debug.Log(charHolder);
        int c = 0;
        characters = new GameObject[numberOfCharacters];
        foreach (Transform child in charHolder.transform)
        {
            characters[c] = child.gameObject;
            c++;
        }
    }

    GameObject FindInActiveObjectByTag(string tag)
    {

        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].CompareTag(tag))
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
}
