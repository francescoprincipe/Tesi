
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SaveTheCatGuy : MonoBehaviour
{
    [SerializeField]
    InputAction JUMP;

    public Vector3 jumpVector = new Vector3(0, 1f, 0);

    [SerializeField]
    public float jumpForce;

    [SerializeField]
    public float gravity;
    public float maxSpeed;
    private Rigidbody rb;

    private bool endGame = false;
    private bool startGame = false;

    private float timeElapsed;
    private float timeRecord = -1;

    [SerializeField]
    private GameObject endPanel;

    [SerializeField]
    private GameObject endTrigger;

    [SerializeField]
    private GameObject recordTextObject;

    [SerializeField]
    private GameObject counterTextObject;

    private TextMeshProUGUI recordText;
    private TextMeshProUGUI counterText;

    [SerializeField]
    private GameObject startingPositionObject;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        JUMP.performed += Jump;
        recordText = recordTextObject.GetComponent<TextMeshProUGUI>();
        counterText = counterTextObject.GetComponent<TextMeshProUGUI>();

    }

    private void OnEnable()
    {
        JUMP.Enable();
        ResetGame();
        startGame = false;
    }

    private void OnDisable()
    {
        JUMP.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (endGame)
            return;


        if (other. gameObject == endTrigger )
        {
            endGame = true;
            endPanel.SetActive(true);
            if (timeElapsed < timeRecord || timeRecord == -1)
            {
                timeRecord = timeElapsed;
                recordText.text = "RECORD: " + timeRecord.ToString("F2") + "s";
            }
        }
    }

    void Update()
    {
        if (endGame || PauseMenu.gamePaused || !startGame)
            return;
        timeElapsed += Time.deltaTime;
        counterText.text = "TEMPO ATTUALE: " + timeElapsed.ToString("F2") + "s";
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }

        if (endGame)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 0);
        }

        if (endGame || PauseMenu.gamePaused || !startGame)
            return;

        rb.AddForce(Vector3.down * gravity * rb.mass);

    }
    void Jump(InputAction.CallbackContext context)
    {
        Debug.Log(endGame.ToString() + startGame.ToString() + PauseMenu.gamePaused.ToString() );

        if (endGame || PauseMenu.gamePaused || !startGame)
            return;

        if (context.phase == InputActionPhase.Performed)
        {
            //Debug.Log("Jump");
            rb.AddForce(jumpVector * jumpForce, ForceMode.Impulse);
        }


    }


    public void StartGame()
    {
        startGame = true;
    }

    public void ResetGame()
    {
        counterText.text = "TEMPO ATTUALE: 0s";
        endGame = false;
        timeElapsed = 0;
        gameObject.transform.position = startingPositionObject.transform.position;
    }
}
