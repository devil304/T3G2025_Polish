using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputActionReference _movementInput;
    [SerializeField] InputActionReference _jumpInput;
    [SerializeField] Rigidbody2D _myRB;
    [SerializeField] float _speedAccel = 0.25f;
    [SerializeField] float _maxHorizontalSpeed = 2f;
    [SerializeField] float _jumpPower = 10f;
    [SerializeField] float _horizontalDrag = 2.5f;
    [SerializeField] Vector2 _groundCheckSize = Vector2.one * 0.9f, _groundCheckOffset = Vector2.zero;
    [SerializeField] LayerMask _groundCheckMask;
    [SerializeField] UnityEvent<GameObject> _OnTouchDanger;
    Vector2 _currentMovement;
    bool _prevJumpState = false;
    bool _isGrounded = false;
    bool _jumpState = false;


    void Start()
    {
        if (_myRB == null || !_myRB)
            _myRB = GetComponent<Rigidbody2D>();
        _movementInput.action.Enable();
        _jumpInput.action.Enable();
        _jumpInput.action.started += Jumping;
        _jumpInput.action.canceled += JumpCanceled;
    }

    private void JumpCanceled(InputAction.CallbackContext context)
    {
        _jumpState = false;
    }

    private void Jumping(InputAction.CallbackContext context)
    {
        _jumpState = true;
    }


    void Update()
    {
        var input = _movementInput.action.ReadValue<Vector2>();
        if (input.x == 0)
        {
            _myRB.linearVelocityX = (Math.Abs(_myRB.linearVelocityX) - (_horizontalDrag * Time.deltaTime)) * Math.Sign(_myRB.linearVelocityX);
        }
        else if (Math.Abs(_myRB.linearVelocityX) < _maxHorizontalSpeed)
        {
            _myRB.linearVelocityX = Math.Clamp(_myRB.linearVelocityX + (input.x * _speedAccel * Time.deltaTime), -_maxHorizontalSpeed, _maxHorizontalSpeed);
        }

        if (_jumpState && !_prevJumpState && _isGrounded)
        {
            _isGrounded = false;
            _myRB.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }
        _prevJumpState = _jumpState;
        var collider = Physics2D.OverlapBox(transform.TransformPoint(_groundCheckOffset), _groundCheckSize, 0, _groundCheckMask);
        if (collider != null)
        {
            if (collider.CompareTag("Ground") && !_prevJumpState)
                _isGrounded = true;
            else if (collider.CompareTag("Danger"))
            {
                _OnTouchDanger?.Invoke(gameObject);
            }
        }
        else
        {
            _isGrounded = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.TransformPoint(_groundCheckOffset), _groundCheckSize);
    }
}
