using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public class Interactable : MonoBehaviour
{
    [System.Serializable]
    public enum INTERACTABLE_TYPE { item, minigame}
    
    
    GameObject highlight;
    GameObject dialogueBox;
    GameObject dialogueManager;
    PhotonView myPV;
    //int playerCounter;


    [SerializeField]
    INTERACTABLE_TYPE interactableType;

    private void OnEnable()
    {
        highlight = transform.GetChild(0).gameObject;
        dialogueBox  = transform.GetChild(1).gameObject;
        dialogueManager = transform.GetChild(2).gameObject;
        myPV = GetComponent<PhotonView>();
        //playerCounter = 0;

    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.tag == "Player" && other.GetType() == typeof(SphereCollider))
        {
            other.GetComponent<DisplayInteractable>().DisplayPlayerUI(true, highlight, dialogueBox, dialogueManager);
            /*
            playerCounter++;
           
            highlight.SetActive(true);
            dialogueManager.SetActive(true);
            dialogueBox.SetActive(true);
            */

        }
        
    }

    private void OnTriggerExit(Collider other)
    {

 
        if (other.tag == "Player" && other.GetType() == typeof(SphereCollider))
        {

            other.GetComponent<DisplayInteractable>().DisplayPlayerUI(false, highlight, dialogueBox, dialogueManager);
            /*
            playerCounter--;
            if (playerCounter == 0)
            {

                dialogueManager.SetActive(false);
                highlight.SetActive(false);
                dialogueBox.SetActive(false);
                
            }
            */
        }
        
    }

    public INTERACTABLE_TYPE GetInteractableType()
    {
        return interactableType;
    }


}
