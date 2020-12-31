using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public abstract class Interactable : MonoBehaviour
{
    [System.Serializable]
    public enum INTERACTABLE_TYPE { item, minigame}
    
    
    protected GameObject highlight;
    protected GameObject dialogueBox;
    protected GameObject dialogueManager;
    PhotonView myPV;



    [SerializeField]
    INTERACTABLE_TYPE interactableType;

    private void OnEnable()
    {
        highlight = transform.GetChild(0).gameObject;
        dialogueBox  = transform.GetChild(1).gameObject;
        dialogueManager = transform.GetChild(2).gameObject;
        myPV = GetComponent<PhotonView>();


    }

    public virtual void OnTriggerEnter(Collider other)
    {


        if (other.tag == "Player" && other.GetType() == typeof(SphereCollider) && other.gameObject.layer == 0)
        {
            other.GetComponent<DisplayInteractable>().DisplayPlayerUI(true, highlight, dialogueBox, dialogueManager);
        }
        
    }

    public virtual void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player" && other.GetType() == typeof(SphereCollider) && other.gameObject.layer == 0)
        {
            other.GetComponent<DisplayInteractable>().DisplayPlayerUI(false, highlight, dialogueBox, dialogueManager);
        }
        
    }

    public INTERACTABLE_TYPE GetInteractableType()
    {
        return interactableType;
    }


}
