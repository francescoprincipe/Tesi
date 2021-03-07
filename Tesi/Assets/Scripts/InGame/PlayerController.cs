using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour, IPunObservable
{
    public static PlayerController localPlayer;
    
    //Components
    Rigidbody myRB;
    Transform myAvatar;
    Animator myAnim;
    [SerializeField] Collider myCollider;
    Camera myCamera;
    GameObject myHUD; //GameObject that contain the canvas for the HUD

    //Inventory
    [SerializeField] Inventory inventory;

    //Input
    [SerializeField] InputAction WASD;
    Vector2 movementInput;
    bool canMove = true;
    //Movement
    [SerializeField] float movementSpeed;
    float direction = 1;

    //Interaction
    [SerializeField] InputAction MOUSE;
    Vector2 mousePositionInput;
    [SerializeField] InputAction INTERACTION;

    //Networking
    PhotonView myPV;

    //Pause
    [SerializeField] InputAction PAUSE;
    GameObject myPause;

    //Chat
    [SerializeField] InputAction CHATMENU;
    [SerializeField] GameObject myChatMenu;
    bool chatMenuEnabled = false;
    [SerializeField] string[] sentences;
    [SerializeField] GameObject myChatCanvasObject;
    [SerializeField] GameObject myDialogueTextAreaObject;
    TextMeshProUGUI myDialogueTextArea;
    int sentenceNumber = -1;
    bool showingMessage = false;


    private void OnEnable()
    {
        WASD.Enable();
        MOUSE.Enable();
        INTERACTION.Enable();
        PAUSE.Enable();
        CHATMENU.Enable();
    }

    private void OnDisable()
    {
        WASD.Disable();
        MOUSE.Disable();
        INTERACTION.Disable();
        PAUSE.Disable();
        CHATMENU.Disable();
    }

    void Awake()
    {
        INTERACTION.performed += Interact;
        PAUSE.performed += Pause;
        CHATMENU.performed += ToggleChatMenu;

        myPV = GetComponent<PhotonView>();
        myCamera = transform.GetChild(1).GetComponent<Camera>(); //Takes the Camera component from the Camera game object attached to the player
        if (myPV.IsMine)
        {
            localPlayer = this;
        }
    }

    void Start()
   
    {

        //Initialization
        myRB = GetComponent<Rigidbody>();
        myAvatar = transform.GetChild(0); //Takes the Sprite transform
        myAnim = GetComponent<Animator>();

        myHUD = transform.GetChild(2).gameObject;
        myPause = GameObject.FindGameObjectWithTag("Pause");
        myDialogueTextArea = myDialogueTextAreaObject.GetComponent<TextMeshProUGUI>();

        if (!myPV.IsMine)
        {
            myCamera.gameObject.SetActive(false);
            myHUD.SetActive(false);
            return;
        }

    }


    void Update()
    {

        //Mirror all avatars on change direction
        myAvatar.localScale = new Vector2(direction, 1);

        if(sentenceNumber != -1 && !showingMessage )
        {
            StartCoroutine(ShowSentence());
        }

        if (!myPV.IsMine || PauseMenu.gamePaused || !canMove)
            return;

        //Get movement input
        movementInput = WASD.ReadValue<Vector2>();

        //Change local avatar direction parameter on received input
        if (movementInput.x != 0)
        {
            direction = Mathf.Sign(movementInput.x);
        }

        //Animation
        myAnim.SetFloat("Speed", movementInput.magnitude);

        //Mouse position
        mousePositionInput = MOUSE.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!myPV.IsMine)
            return;

        myRB.velocity = movementInput * movementSpeed;
    }

    //Getters
    public Camera GetCamera()
    {
        if (!myPV.IsMine)
            return null;

        return myCamera;
    }

    public PhotonView GetPhotonView()
    {
        if (!myPV.IsMine)
            return null;

        return myPV;
    }

    public Inventory GetInventory()
    {
        if (!myPV.IsMine)
            return null;

        return inventory;
    }



    //IPUNOBservable Method
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(direction);
            //Debug.Log("Sent direction " + myPV.ViewID);
            stream.SendNext(sentenceNumber);
            //Debug.Log("Sent sentence number " + sentenceNumber +" "+ myPV.ViewID );
        }
        else
        {
            this.direction = (float)stream.ReceiveNext();
            //Debug.Log("Received direction " + myPV.ViewID);
            this.sentenceNumber = (int)stream.ReceiveNext();
            //Debug.Log("Received sentence number " + sentenceNumber + " " + myPV.ViewID);
        }
    }

    //Actions binded to keys
    void Interact(InputAction.CallbackContext context)
    {

        if(context.phase == InputActionPhase.Performed && myPV.IsMine && !PauseMenu.gamePaused && canMove)
        {
            RaycastHit hit;
            Ray ray = myCamera.ScreenPointToRay(mousePositionInput);
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.tag == "Interactable")
                {
                    Debug.DrawRay(myCamera.ScreenToWorldPoint (mousePositionInput), hit.point, Color.green, 5f);

                    if (!hit.transform.GetChild(0).gameObject.activeInHierarchy) // If Highlight component of the Interactable is not active return
                        return;

                    Interactable hitObject = hit.transform.GetComponent<Interactable>();
                    Interactable.INTERACTABLE_TYPE interactableType = hitObject.GetInteractableType();
                    PerformInteraction(hit.transform, interactableType);

 
                }
            }
        }
    }
    void PerformInteraction(Transform target, Interactable.INTERACTABLE_TYPE type)
    {
        switch (type)
        {
            case Interactable.INTERACTABLE_TYPE.item:
                Item targetItem = target.GetComponent<Item>();
                string name = targetItem.GetName();
                Sprite sprite = targetItem.GetInventorySprite();
                inventory.AddItem(name, sprite);
                break;

            case Interactable.INTERACTABLE_TYPE.minigame:
                Minigame targetMinigame = target.GetComponent<Minigame>();
                if (!targetMinigame.CheckQuestCompleted())
                    return;
                Debug.Log("Quest completata");
                targetMinigame.PlayMinigame();
                break;
            default: break;
        }
    }

    void Pause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && myPV.IsMine)
        {
            myPause.GetComponent<PauseMenu>().TogglePause();

        }
    }

    void ToggleChatMenu(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && myPV.IsMine)
        {
            chatMenuEnabled = !chatMenuEnabled;
            myChatMenu.SetActive(chatMenuEnabled);
        }
    }

    //Chat functions 
    public void SetSentence(int number)
    {
        sentenceNumber = number - 1;
        chatMenuEnabled = !chatMenuEnabled;
        myChatMenu.SetActive(chatMenuEnabled);
    }

    public IEnumerator ShowSentence()
    {
        showingMessage = true;
        myDialogueTextArea.text = sentences[sentenceNumber];
        myChatCanvasObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        myChatCanvasObject.SetActive(false);
        sentenceNumber = -1;
        showingMessage = false;
    }

    //Utility functions
    public void ToggleHUD(bool active)
    {
        myHUD.SetActive(active);
    }

    public void ToggleInput(bool active)
    {
        canMove = active;
    }

    public void CheckQuest(string item, ref bool questCompleted)
    {
        if (!myPV.IsMine)
            return;
        questCompleted = inventory.HaveItem(item);
    }

}
