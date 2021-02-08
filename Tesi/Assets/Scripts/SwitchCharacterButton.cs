using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCharacterButton : MonoBehaviour
{

    public void Next()
    {
        OptionsManager.Instance.GetComponent<OptionsManager>().GoNextCharacter();
    }
    public void Previous()
    {
        OptionsManager.Instance.GetComponent<OptionsManager>().GoPreviousCharacter();
    }
}
