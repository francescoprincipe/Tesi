using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.IO;

public class MyPhotonPlayer : MonoBehaviour
{
    PhotonView myPV;
    GameObject myAvatar;
    GameObject[] interactables;
 

    Player[] allPlayers;
    int myNumberInRoom;

    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
        allPlayers = PhotonNetwork.PlayerList;
        myNumberInRoom = 0;
        interactables = GameObject.FindGameObjectsWithTag("Interactable");
    }
    void Start()
    {

        foreach (Player player in allPlayers)
        {
            if(player != PhotonNetwork.LocalPlayer)
            {
                myNumberInRoom++;
            }
        }

        if (myPV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), GameManager.Instance.spawnPoints[myNumberInRoom].position, Quaternion.identity);
            foreach(GameObject interactable in interactables)
            {
                interactable.transform.GetChild(1).GetComponent<Canvas>().worldCamera = PlayerController.localPlayer.GetCamera();
            }
        }
    }

}
