using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;

public class BlindfoldGuy : MonoBehaviour
{
    Rigidbody myRB;
    Transform myAvatar;
    Animator myAnim;

    private bool endGame = false;
    private bool startGame = false;

    [SerializeField] private GameObject startingPositionObject;
    [SerializeField] private GameObject endPanel;

    [SerializeField] InputAction WASD;
    Vector2 movementInput;
    [SerializeField] float movementSpeed;
    float direction = 1;

    private List<GameObject> targetsFound;
    private int targetsCounter;
    [SerializeField] private List<GameObject> targets;
    [SerializeField] private List<GameObject> startingPositions;

    [SerializeField] private GameObject counterTextObject;
    private TextMeshProUGUI counterText;

    private void OnEnable()
    {
        WASD.Enable();
        targetsFound = new List<GameObject>();
        ResetGame();
        startGame = false;
    }

    private void OnDisable()
    {
        WASD.Disable();
    }

    private void Awake()
    {
        myRB = GetComponent<Rigidbody>();
        myAvatar = transform.GetChild(0); //Takes the Sprite transform
        myAnim = GetComponent<Animator>();
        counterText = counterTextObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        myAvatar.localScale = new Vector2(direction, 1);
        if (!startGame || endGame || PauseMenu.gamePaused)
            return;


        movementInput = WASD.ReadValue<Vector2>();
        if (movementInput.x != 0)
        {
            direction = Mathf.Sign(movementInput.x);
        }

        myAnim.SetFloat("Speed", movementInput.magnitude);
        // myAnim.SetFloat("Speed", movementInput.magnitude);
    }

    private void FixedUpdate()
    {
        if (!startGame || endGame || PauseMenu.gamePaused)
            return;
        myRB.velocity = movementInput * movementSpeed;
    }

    public void ResetGame()
    {
        endGame = false;
        targetsFound = new List<GameObject>();
        targetsCounter = 0;
        counterText.text = "AMICI TROVATI: 0";
        this.transform.position = startingPositionObject.transform.position;
        int c = 0;
        var rnd = new System.Random();
        List<GameObject> result = new List<GameObject>(startingPositions.OrderBy(item => rnd.Next()));
        foreach (GameObject target in targets)
        {
            target.transform.position = result[c].transform.position;
            c++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (endGame)
            return;

        if (targets.Contains(other.gameObject))
        {
            if (!targetsFound.Contains(other.gameObject))
            {
                targetsFound.Add(other.gameObject);
                targetsCounter += 1;
                counterText.text = "AMICI TROVATI: " + targetsCounter.ToString();

                if (targetsFound.Count == targets.Count)
                {
                    endGame = true;
                    endPanel.SetActive(true);
                    myRB.velocity = Vector3.ClampMagnitude(myRB.velocity, 0);
                }
            }
        }
    }

    public void StartGame()
    {
        startGame = true;
    }
}
