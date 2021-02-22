using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perspective : MonoBehaviour
{
    SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if(other.transform.position.y > this.transform.position.y)
            {
                other.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder - 1;
                
            }
            else
            {
                other.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder + 1;
            }
        }
    }
}
