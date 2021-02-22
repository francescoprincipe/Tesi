using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : Interactable
{
    [SerializeField]
    public string itemRequired;

    bool isPlaying = false;

    private bool questCompleted = false;

    [SerializeField]
    private GameObject minigame;

    [SerializeField]
    private GameObject instructionsPanel;

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().CheckQuest(itemRequired, ref questCompleted);
            dialogueManager.GetComponent<DialogueManager>().SetDialogue(questCompleted);
        }
        base.OnTriggerEnter(other);
    }

     private IEnumerator RunMinigame()
    {
        PlayerController.localPlayer.ToggleInput(false);
        PlayerController.localPlayer.ToggleHUD(false);
        highlight.SetActive(false);
        isPlaying = true;
        minigame.SetActive(true);
        if(instructionsPanel != null)
            instructionsPanel.SetActive(true);
        minigame.GetComponent<Canvas>().worldCamera = PlayerController.localPlayer.GetCamera();
        minigame.GetComponent<Canvas>().planeDistance = 1;
        minigame.GetComponent<Canvas>().sortingLayerName = "Minigame";
        Debug.Log("Minigame activated");
        yield return new WaitUntil(() => isPlaying == false);
        highlight.SetActive(true);
        PlayerController.localPlayer.ToggleHUD(true);
        PlayerController.localPlayer.ToggleInput(true);
    }

    public void PlayMinigame()
    {
        StartCoroutine("RunMinigame");
    }
    public void CloseMinigame()
    {
        minigame.SetActive(false);
        isPlaying = false;
    }

    public bool CheckQuestCompleted()
    {
        return questCompleted;
    }

}
