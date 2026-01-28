using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";
    private const string JumpButton = "Jump";
    private const float TikcTome = 60f;

    [SerializeField] private float _inputSmoothing = 0.1f;
    [SerializeField] private bool _rawInput = false;

    public event Action<Vector2> OnMoveInput;
    public event Action OnJumpPressed;
    public event Action OnJumpReleased;

    private Vector2 _currentMoveInput;
    private bool _isJumpPressed;

    public Vector2 SmoothedMoveInput { get; private set; }

    private void Update()
    {
        HandleAllInputs();
        SmoothInput();
    }

    private void HandleAllInputs()
    {
        Vector2 newMoveInput;

        if (_rawInput)
            newMoveInput = new Vector2(Input.GetAxisRaw(HorizontalAxis), Input.GetAxisRaw(VerticalAxis));
        else
            newMoveInput = new Vector2(Input.GetAxis(HorizontalAxis), Input.GetAxis(VerticalAxis));

        if (newMoveInput != _currentMoveInput)
        {
            _currentMoveInput = newMoveInput;
            OnMoveInput?.Invoke(_currentMoveInput);
        }

        bool jumpPressed = Input.GetButtonDown(JumpButton);
        bool jumpReleased = Input.GetButtonUp(JumpButton);

        if (jumpPressed)
        {
            _isJumpPressed = true;
            OnJumpPressed?.Invoke();
        }

        if (jumpReleased)
        {
            _isJumpPressed = false;
            OnJumpReleased?.Invoke();
        }
    }

    private void SmoothInput()
    {
        SmoothedMoveInput = Vector2.Lerp(SmoothedMoveInput, _currentMoveInput, _inputSmoothing * Time.deltaTime * TikcTome);

        if (SmoothedMoveInput.magnitude > 1f)
            SmoothedMoveInput = SmoothedMoveInput.normalized;
    }
}