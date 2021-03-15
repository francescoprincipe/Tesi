using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class DrawManager : MonoBehaviour
{
    private Camera drawCamera;
    public Canvas canvas;
    public GameObject brush;


    LineRenderer currentLineRenderer;
    //Material material;
    Color color;
    public float width;
    public Color[] availableColors;

    Vector2 lastPosition;

    [SerializeField] InputAction MOUSE;
    [SerializeField] InputAction DRAW;
    Vector2 mousePositionInput;

    bool drawButton = false;
    bool start = false;

    private void OnEnable()
    {
        MOUSE.Enable();
        DRAW.Enable();
        Clear();
    }

    private void OnDisable()
    {
        MOUSE.Disable();
        DRAW.Disable();
    }

    private void Awake()
    {
        DRAW.performed += OnDraw;
        DRAW.canceled += OnDraw;
        drawButton = false;
        start = false;
        color = Color.black;
    }
    private void Start()
    {
        drawCamera = canvas.worldCamera;
    }

    private void Update()
    {
        if (!start || PauseMenu.gamePaused)
            return;

        mousePositionInput = MOUSE.ReadValue<Vector2>();
        Draw();
    }

    void Draw()
    {

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            CreateBrush();
        }

        if (drawButton)
        {
            Vector2 mousePos = drawCamera.ScreenToWorldPoint(mousePositionInput);
            if(mousePos != lastPosition)
            {
                AddPoint(mousePos);
                lastPosition = mousePos;
            }
        }
        else
        {
            currentLineRenderer = null;
        }
    }

    void CreateBrush()
    {
        GameObject brushInstance = Instantiate(brush);
        brushInstance.transform.parent = this.gameObject.transform;
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
        currentLineRenderer.startColor = color;
        currentLineRenderer.endColor = color;
        currentLineRenderer.startWidth = width; ;
        Vector2 mousePos = drawCamera.ScreenToWorldPoint(mousePositionInput);

        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);
    }

    void AddPoint(Vector2 pointPos)
    {
        try
        {
            currentLineRenderer.positionCount++;
            int positionIndex = currentLineRenderer.positionCount - 1;
            currentLineRenderer.SetPosition(positionIndex, pointPos);
        }
        catch(NullReferenceException e)
        {
            Debug.Log("Nessun line renderer trovato");
        }
    }

    private void OnDraw(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Performed)
        {
            drawButton = true;
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            drawButton = false;
        }

    }

    public void StartDraw()
    {
        start = true;
    }

    public void SetColor(int colorIndex)
    {
        this.color = availableColors[colorIndex];
    }

    public void Clear()
    {
        foreach(Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
