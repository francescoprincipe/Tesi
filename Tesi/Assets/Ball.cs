using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    Rigidbody rb;

    private bool endGame = false;
    private bool startGame = false;

    [SerializeField] private GameObject shootingGuy;
    [SerializeField] private GameObject shootingGirl;

    [SerializeField] private GameObject startingPositionObject;
    [SerializeField] private GameObject[] startingTargetPositionObject;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject trigger;
    [SerializeField] InputAction THROW;
    
    bool throwButton = false;
    [SerializeField] private float maxForce;
    private float actualForce;
    [SerializeField] private float updateRate;
    Vector3 shootVector = new Vector3(1f, 0f, 0);

    [SerializeField] private GameObject recordTextObject;
    private TextMeshProUGUI recordText;

    [SerializeField] private GameObject streakTextObject;
    private TextMeshProUGUI streakText;

    [SerializeField] private Slider charge;

    private int record = 0;
    private int streak;

    private void OnEnable()
    {
        THROW.Enable();
        ResetGame();
        streak = 0;
        streakText.text = "CENTRI DI FILA: 0";
    }

    private void OnDisable()
    {
        THROW.Disable();
    }

    private void Awake()
    {
        if (OptionsManager.Instance.characterSelected == 0)
            shootingGirl.SetActive(false);
        else
            shootingGuy.SetActive(false);

        THROW.performed += OnThrow;
        THROW.canceled += OnThrow;
        throwButton = false;
        startGame = false;
        rb = GetComponent<Rigidbody>();
        recordText = recordTextObject.GetComponent<TextMeshProUGUI>();
        streakText = streakTextObject.GetComponent<TextMeshProUGUI>();
        

    }

    private void Update()
    {
        if (!startGame || PauseMenu.gamePaused || endGame)
            return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine(ChargeShoot());
        }

        if (Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            ToggleLock(false);
            Shoot();
        }



    }
    private void OnTriggerEnter(Collider other)
    {
        if (endGame)
            return;

        if(other.tag == "Floor")
        {
            endGame = true;
            ResetGame();
            streak = 0;
            streakText.text = "CENTRI DI FILA: 0";
        }

        if(other.gameObject == trigger)
        {
            streak++;
            streakText.text = "CENTRI DI FILA: " + streak.ToString();
            if(streak > record)
            {
                record = streak;
                recordText.text = "RECORD: " + record.ToString();
            }
            ResetGame();
        }
    }
    IEnumerator ChargeShoot()
    {
        charge.gameObject.SetActive(true);
        while (throwButton)
        {
            actualForce += maxForce * 0.05f;
            if (actualForce > maxForce)
                actualForce = maxForce;
            charge.value = actualForce / maxForce;
            yield return new WaitForSeconds(updateRate);
        }
        charge.gameObject.SetActive(false);
    }

    private void OnThrow(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Performed)
        {
            throwButton = true;
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            throwButton = false;
        }

    }
    private void Shoot()
    {
        rb.AddForce(shootVector * actualForce, ForceMode.Impulse);
    }

    private void ToggleLock(bool rbLock)
    {
        if (rbLock)
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        else
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
        }
    }

    public void StartGame()
    {
        startGame = true;
    }

    public void ResetGame()
    {
        endGame = false;
        actualForce = 0f;
        gameObject.transform.position = startingPositionObject.transform.position;
        int rand = Random.Range(0, startingTargetPositionObject.Length - 1);
        target.transform.position = startingTargetPositionObject[rand].transform.position;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 0);
        ToggleLock(true);


    }
}
