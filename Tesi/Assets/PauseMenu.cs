using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviourPunCallbacks
{

    public static bool gamePaused = false;
    private bool disconnecting = false;
    [SerializeField]
    GameObject pauseMenuUI;

    public void TogglePause()
    {
        if (disconnecting)
            return;

        gamePaused = !gamePaused;
        pauseMenuUI.SetActive(gamePaused);

    }

    public void Quit()
    {
        disconnecting = true;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);

        base.OnLeftRoom();
    }
}
