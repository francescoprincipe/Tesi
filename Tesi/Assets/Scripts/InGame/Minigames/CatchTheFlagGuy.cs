using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CatchTheFlagGuy : MonoBehaviour
{
    [SerializeField]
    InputAction RUN;

    public Vector3 runVector = new Vector3(1f, 0f, 0);

    [SerializeField]
    public float runForce;

    public float maxSpeed;
    private Rigidbody rb;
    private Animator myAnim;

    private bool endGame = false;
    private bool startGame = false;

    [SerializeField]
    bool girl;

    [SerializeField]
    private bool human;
    [SerializeField]
    private float runningRateBot;

    [SerializeField]
    private GameObject startingPositionObject;


    private void Awake()
    {
        if ((OptionsManager.Instance.characterSelected == 0 && girl && human) || (OptionsManager.Instance.characterSelected == 1 && !girl && human))
            this.gameObject.SetActive(false);

        myAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        if (human)
            RUN.performed += Run;

    }

    private void OnEnable()
    {
        if(human)
            RUN.Enable();
        ResetGame();
        startGame = false;
    }

    private void OnDisable()
    {
        if (human)
            RUN.Disable();
    }



    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }

        if (endGame || PauseMenu.gamePaused)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 0);
        }

        myAnim.SetFloat("Speed", rb.velocity.magnitude);

    }
    void Run(InputAction.CallbackContext context)
    {

        if (endGame || PauseMenu.gamePaused || !startGame)
            return;

        if (context.phase == InputActionPhase.Performed)
        {
            KeepRunning();
        }

    }

    private IEnumerator BotRunning()
    {
        while (true)
        {
            yield return new WaitForSeconds(runningRateBot);
            if (startGame && !PauseMenu.gamePaused)
            {
                KeepRunning();
            }
            
        }
    }
    private void KeepRunning()
    {
        rb.AddForce(runVector * runForce, ForceMode.Impulse);
    }

    public void StartGame()
    {
        startGame = true;
        if (!human)
        {
            StopAllCoroutines();
            StartCoroutine(BotRunning());
        }
    }

    public void EndGame()
    {
        endGame = true;
        if (!human)
        {
            StopAllCoroutines();
        }
    }

    public void ResetGame()
    {
        endGame = false;
        gameObject.transform.position = startingPositionObject.transform.position;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 0);

        if (!human)
        {
            StopAllCoroutines();
            StartCoroutine(BotRunning());
        }
    }
}