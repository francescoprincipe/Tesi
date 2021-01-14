using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatButton : MonoBehaviour
{
    void OnEnable()
    {
       this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }


}
