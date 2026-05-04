using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Configuración de Cámara")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private PlayerInputReader input; // ✅ Clase concreta (NO interfaz)
    [SerializeField] private Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
        // Bloquea y oculta el cursor al iniciar
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (input == null || playerBody == null)
        {
            Debug.LogWarning("[CameraController] Faltan referencias en el Inspector.");
            return;
        }

        // Leer el movimiento del ratón
        Vector2 lookInput = input.LookInput;

        // Aplicar sensibilidad y Time.deltaTime para movimiento suave
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        // 🔄 Rotación vertical (solo la cámara)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limita vista arriba/abajo
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 🔄 Rotación horizontal (rota todo el cuerpo del jugador)
        playerBody.Rotate(Vector3.up * mouseX);
    }

    // 🔒 Restaurar cursor si la ventana pierde el foco (Alt+Tab, pausas, etc.)
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