using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [System.Serializable]
    public enum INTERACTABLE_TYPE { item, minigame}
    
    
    GameObject highlight;

    [SerializeField]
    INTERACTABLE_TYPE interactableType;

    private void OnEnable()
    {
        highlight = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            highlight.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            highlight.SetActive(false);
        }
    }

    public INTERACTABLE_TYPE GetInteractableType()
    {
        return interactableType;
    }
}
