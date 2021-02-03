using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PaperPlane : MonoBehaviour
{
    Rigidbody2D rb;
    Transform myAvatar;
    Animator myAnim;

    private bool endGame = false;
    private bool startGame = false;

    [SerializeField] private GameObject startingPositionObject;
    [SerializeField] private GameObject landingPosition;
    [SerializeField] private GameObject endPanel;

    [SerializeField] InputAction WASD;
    Vector2 movementInput;
    private float currentSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float steering;



    [SerializeField] private GameObject timerTextObject;
    private TextMeshProUGUI timerText;
    [SerializeField] private GameObject recordTextObject;
    private TextMeshProUGUI recordText;
    private float timeElapsed;
    private float record = -1;

    private void OnEnable()
    {
        WASD.Enable();
        ResetGame();
        startGame = false;
        endGame = false;
    }

    private void OnDisable()
    {
        WASD.Disable();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myAvatar = transform.GetChild(0); //Takes the Sprite transform
        timerText = timerTextObject.GetComponent<TextMeshProUGUI>();
        recordText = recordTextObject.GetComponent<TextMeshProUGUI>();
    }


    private void FixedUpdate()
    {
        
        if (!startGame || endGame || PauseMenu.gamePaused)
            return;
        timeElapsed += Time.deltaTime;
        timerText.text = "TEMPO ATTUALE: " + timeElapsed.ToString("F2") + "s";

        movementInput = WASD.ReadValue<Vector2>();
        float h = -movementInput.x;
        float v = movementInput.y;

        // Calculate speed from input and acceleration (transform.up is forward)
        Vector2 speed = transform.up * (v * acceleration);
        rb.AddForce(speed);

        // Create car rotation
        float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));
        if (direction >= 0.0f)
        {
            rb.rotation += h * steering * (rb.velocity.magnitude / maxSpeed);
        }
        else
        {
            rb.rotation -= h * steering * (rb.velocity.magnitude / maxSpeed);
        }

        // Change velocity based on rotation
        float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.left)) * 2.0f;
        Vector2 relativeForce = Vector2.right * driftForce;
       // Debug.DrawLine(rb.position, rb.GetRelativePoint(relativeForce), Color.green);
        rb.AddForce(rb.GetRelativeVector(relativeForce));

        // Force max speed limit
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        currentSpeed = rb.velocity.magnitude;
    }

    public void ResetGame()
    {
        endGame = false;
        timerText.text = "TEMPO: 0s";
        timeElapsed = 0;
        this.transform.position = startingPositionObject.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        if (endGame)
            return;
        
        if (other.gameObject == landingPosition)
        {
            endGame = true;
            endPanel.SetActive(true);
            if (timeElapsed < record || record == -1)
            {
                record = timeElapsed;
                recordText.text = "RECORD: " + timeElapsed.ToString(); 

            }
        }
        else
        if (other.tag == "Floor")
        {
            ResetGame();
        }

    }

    public void StartGame()
    {
        startGame = true;
    }
}
