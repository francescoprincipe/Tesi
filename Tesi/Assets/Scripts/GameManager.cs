using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField]
    Transform[] spawnPoints;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
 
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);

    }


    void Update()
    {
        
    }
}
