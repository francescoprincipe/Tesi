using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpingGuy : MonoBehaviour
{
    [SerializeField] 
    InputAction JUMP;

    [SerializeField]
    bool girl;

    public Vector3 jumpVector = new Vector3(0, 1f, 0);

    [SerializeField]
    public float jumpForce;

    [SerializeField]
    public float gravity;

    private bool isGrounded;
    private Rigidbody rb;


    private void Awake()
    {
        if ((OptionsManager.Instance.characterSelected == 0 && girl) || (OptionsManager.Instance.characterSelected == 1 && !girl))
            this.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody>();
        JUMP.performed += Jump;
    }

    private void OnEnable()
    {
        JUMP.Enable();
    }

    private void OnDisable()
    {
        JUMP.Disable();
    }

    public void OnTriggerStay(Collider collider)
    {

        if(collider.tag == "Floor" && collider.gameObject.layer == 8)
        {
            isGrounded = true;
            //Debug.Log("IsGrounded");
        }
    }

    public void OnTriggerExit(Collider collider)
    {

        if (collider.tag == "Floor" && collider.gameObject.layer == 8)
        {
            isGrounded = false;
            Debug.Log("NotGrounded");
        }
    }

    void Jump(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Performed && isGrounded )
        {
            Debug.Log("Jump");
            rb.AddForce(jumpVector * jumpForce, ForceMode.Impulse);
        }


    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.down * gravity * rb.mass);
    }
}
