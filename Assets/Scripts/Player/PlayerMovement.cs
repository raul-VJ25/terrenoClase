using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    [Header("Salto y Gravedad")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Detector de Suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController _controller;
    private IPlayerInput _input;
    private Vector3 _velocity;

    [SerializeField] private bool _isGrounded;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<IPlayerInput>();
    }

    void Update()
    {
        if (_input == null) return;

        if (_input.isJumping)
        {
            Debug.Log("salta");
        }

        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Vector3 moveDirection = transform.right * _input.MoveInput.x + transform.forward * _input.MoveInput.y;
        _controller.Move(moveDirection * speed * Time.deltaTime);

        if (_isGrounded && _velocity.y < 0) _velocity.y = -2f;
        if (_input.isJumping && _isGrounded)
        {
            Debug.Log("jumpHeight");
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}
