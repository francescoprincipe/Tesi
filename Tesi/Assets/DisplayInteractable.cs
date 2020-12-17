using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DisplayInteractable : MonoBehaviourPun
{
    PhotonView myPV;
    void Awake()
    {
        myPV = GetComponent<PhotonView>();
    }
    public void DisplayPlayerUI(bool active, GameObject highlight, GameObject dialogueBox, GameObject dialogueManager)
    {
        if (!myPV.IsMine) return;

        highlight.SetActive(active);
        dialogueBox.SetActive(active);
        dialogueManager.SetActive(active);
    }
}
