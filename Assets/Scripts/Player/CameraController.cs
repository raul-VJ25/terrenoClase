using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Configuración de Cámara")]
    [SerializeField] private float mouseSensitivity = 100f;

    private IPlayerInput _input;

    [SerializeField] private Transform playerBody;
    private float xRotation = 0f;

    void Awake()
    {
        _input = GetComponentInParent<IPlayerInput>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (_input == null || playerBody == null)
        {
            Debug.LogWarning("[CameraController] Faltan referencias. Asegúrate de que el PlayerInputReader está en un objeto padre de la cámara.");
            return;
        }

        Vector2 lookInput = _input.LookInput;

        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}