using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RopeRotation : MonoBehaviour
{
    public float startingSpeed;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float speed;

    private int jumpCounter;
    private int record;

    private bool endGame;
    private bool startGame;

    [SerializeField]
    private GameObject endPanel;

    [SerializeField]
    private GameObject halfLapTrigger;

    [SerializeField]
    private GameObject lapTrigger;

    [SerializeField]
    private GameObject recordTextObject;

    [SerializeField]
    private GameObject counterTextObject;

    private TextMeshProUGUI recordText;
    private TextMeshProUGUI counterText;

    Quaternion initRotation = new Quaternion(180, 0, 0, 0);
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        recordText = recordTextObject.GetComponent<TextMeshProUGUI>();
        counterText = counterTextObject.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        ResetGame();
        startGame = false;
    }

    void Update()
    {
        if (endGame || PauseMenu.gamePaused || !startGame)
            return;

        transform.RotateAround(transform.position, Vector3.right, Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (endGame)
            return;

        if(other.gameObject == lapTrigger)
        {
            speed -= 15;
            spriteRenderer.sortingOrder = 2;
        }

        if (other.gameObject == halfLapTrigger)
        {
            jumpCounter += 1;
            spriteRenderer.sortingOrder = 0;
            counterText.text = "NUMERO DI SALTI: " + jumpCounter;
            if (jumpCounter > record)
            {
                record = jumpCounter;
                recordText.text = "RECORD: " + record;
            }
                
        }

        if(other.tag == "MinigamePlayer")
        {
            endGame = true;
            endPanel.SetActive(true);
        }
    }

    public void StartGame()
    {
        startGame = true;
    }

    public void ResetGame()
    {
        speed = startingSpeed;
        jumpCounter = 0;
        counterText.text = "NUMERO DI SALTI: 0";
        endGame = false;
        this.gameObject.transform.rotation = initRotation;
    }
}
