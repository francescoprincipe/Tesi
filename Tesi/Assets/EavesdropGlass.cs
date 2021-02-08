using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class EavesdropGlass : MonoBehaviour
{
    Rigidbody myRB;

    private bool endGame = false;
    private bool startGame = false;

    [SerializeField] private GameObject startingPositionObject;
    [SerializeField] private GameObject endPanel;

    [SerializeField] InputAction WASD;
    Vector2 movementInput;
    [SerializeField] float movementSpeed;

    [SerializeField] private float maxDistance;
    private float minigameVolume;

    [SerializeField] private GameObject target;
    [SerializeField] private GameObject[] startingTargetPositionObject;

    [SerializeField] private GameObject counterTextObject;
    private TextMeshProUGUI counterText;

    private void OnEnable()
    {
        WASD.Enable();
        ResetGame();
        startGame = false;
        AudioManager.Instance.PauseSound("GameSound");
        AudioManager.Instance.PlaySound("EavesdropMinigameSound");
        AudioManager.Instance.SetSoundVolume("EavesdropMinigameSound", 0);
    }

    private void OnDisable()
    {
        WASD.Disable();
        AudioManager.Instance.UnPauseSound("GameSound");
        AudioManager.Instance.StopSound("EavesdropMinigameSound");
    }

    private void Awake()
    {
        myRB = GetComponent<Rigidbody>();
        counterText = counterTextObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {

        if (!startGame || endGame || PauseMenu.gamePaused)
            return;


        movementInput = WASD.ReadValue<Vector2>();

    }

    private void FixedUpdate()
    {
        if (!startGame || endGame || PauseMenu.gamePaused)
            return;
        myRB.velocity = movementInput * movementSpeed;
        float distance = Vector3.Distance(target.transform.position, this.transform.position);
        float ratio = distance / maxDistance;
        if (ratio > 1)
            ratio = 1;
        AudioManager.Instance.SetSoundVolume("EavesdropMinigameSound", 1 - ratio);
    }

    public void ResetGame()
    {
        endGame = false;
        counterText.text = "AMICI TROVATI: 0";
        this.transform.position = startingPositionObject.transform.position;
        int rand = Random.Range(0, startingTargetPositionObject.Length - 1);
        target.transform.position = startingTargetPositionObject[rand].transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (endGame)
            return;

        if (other.gameObject == target)
        {
          endGame = true;
          endPanel.SetActive(true);
          myRB.velocity = Vector3.ClampMagnitude(myRB.velocity, 0);
        }
    }

    public void StartGame()
    {
        startGame = true;
    }
}
