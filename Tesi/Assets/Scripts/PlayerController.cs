﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;

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
    [SerializeField]
    Inventory inventory;

    //Input
    [SerializeField] InputAction WASD;
    Vector2 movementInput;

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



    private void OnEnable()
    {
        WASD.Enable();
        MOUSE.Enable();
        INTERACTION.Enable();
        PAUSE.Enable();
    }

    private void OnDisable()
    {
        WASD.Disable();
        MOUSE.Disable();
        INTERACTION.Disable();
        PAUSE.Disable();
    }

    void Awake()
    {
        INTERACTION.performed += Interact;
        PAUSE.performed += Pause;
    }

    void Start()
   
    {
        myPV = GetComponent<PhotonView>();

        if (myPV.IsMine)
        {
            localPlayer = this;
        }

        //Initialization
        myRB = GetComponent<Rigidbody>();
        myAvatar = transform.GetChild(0); //Takes the Sprite transform
        myAnim = GetComponent<Animator>();
        myCamera = transform.GetChild(1).GetComponent<Camera>(); //Takes the Camera component from the Camera game object attached to the player
        myHUD = transform.GetChild(2).gameObject;
        myPause = GameObject.FindGameObjectWithTag("Pause");

        if (!myPV.IsMine)
        {
            myCamera.gameObject.SetActive(false);
            return;
        }

    }


    void Update()
    {
        //Mirror all avatars on change direction
        myAvatar.localScale = new Vector2(direction, 1);

        if (!myPV.IsMine)
            return;

        //Get input
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(direction);
        }
        else
        {
            direction = (float)stream.ReceiveNext();
        }
    }

    void Interact(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed && myPV.IsMine)
        {
            RaycastHit hit;
            Ray ray = myCamera.ScreenPointToRay(mousePositionInput);
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.tag == "Interactable")
                {
                    if (!hit.transform.GetChild(0).gameObject.activeInHierarchy) // If Highlight component of the Interactable is not active return
                        return;

                    Interactable hitObject = hit.transform.GetComponent<Interactable>();
                    Interactable.INTERACTABLE_TYPE interactableType = hitObject.GetInteractableType();
 
                }
            }
        }
    }

    void Pause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && myPV.IsMine)
        {
            myPause.GetComponent<PauseMenu>().TogglePause();

        }
    }
}
