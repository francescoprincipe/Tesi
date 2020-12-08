using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager:MonoBehaviour
{

    [SerializeField]
    private LobbyManager lobbyManager;


    public void Quit()
    {
        Application.Quit();
    }

    public void ChangeMenu( GameObject requestedMenu)
    {
        GameObject[] menus = GameObject.FindGameObjectsWithTag("Menu");
        foreach (GameObject menu in menus)
            menu.SetActive(false);
        requestedMenu.SetActive(true);
    }



}
