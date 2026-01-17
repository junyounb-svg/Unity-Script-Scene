using UnityEngine;
using UnityEngine.InputSystem;

public class Clickscript : MonoBehaviour
{
    [Header("Color Settings")]
    [SerializeField] private Color[] colors = new Color[]
    {
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        Color.magenta,
        Color.cyan,
        new Color(1f, 0.5f, 0f), // Orange
        new Color(0.5f, 0f, 1f), // Purple
        Color.black
    };

    private int currentColorIndex = 0;
    private Renderer cubeRenderer;
    private Camera mainCamera;

    void Start()
    {
        // Get the Renderer component
        cubeRenderer = GetComponent<Renderer>();
        if (cubeRenderer == null)
        {
            cubeRenderer = GetComponentInChildren<Renderer>();
        }

        if (cubeRenderer == null)
        {
            Debug.LogError("Clickscript: No Renderer found on " + gameObject.name);
        }

        // Get the main camera
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            mainCamera = FindObjectOfType<Camera>();
        }

        if (mainCamera == null)
        {
            Debug.LogError("Clickscript: No Camera found in scene!");
        }

        // Ensure there's a collider for raycasting
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            col = GetComponentInChildren<Collider>();
        }
        
        if (col == null)
        {
            Debug.LogWarning("Clickscript: No Collider found. Adding BoxCollider...");
            gameObject.AddComponent<BoxCollider>();
        }
        else
        {
            Debug.Log("Clickscript: Collider found: " + col.GetType().Name);
        }
        
        Debug.Log("Clickscript initialized on " + gameObject.name);
    }

    void Update()
    {
        // Check for mouse click using new Input System
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
                return;
            }

            // Cast a ray from camera through mouse position
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
                
                // Check if this cube was clicked
                if (hit.collider.gameObject == gameObject || 
                    hit.collider.transform.IsChildOf(transform) ||
                    hit.collider.transform == transform)
                {
                    Debug.Log("Cube clicked! Changing color...");
                    ChangeColor();
                }
            }
            else
            {
                Debug.Log("Raycast hit nothing");
            }
        }
    }

    void ChangeColor()
    {
        if (cubeRenderer != null)
        {
            // Cycle to the next color
            currentColorIndex = (currentColorIndex + 1) % colors.Length;
            cubeRenderer.material.color = colors[currentColorIndex];
            Debug.Log("Color changed to: " + colors[currentColorIndex]);
        }
        else
        {
            Debug.LogError("Cannot change color - Renderer is null!");
        }
    }
}
