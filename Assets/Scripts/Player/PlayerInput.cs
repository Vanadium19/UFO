using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{   
    private Vector2 _moveInput;

    public event Action CatchPressed;
    public event Action CatchReleased;

    public Vector2 Controls => _moveInput;

    private void Update()
    {
        _moveInput = Vector2.up * Input.GetAxis("Vertical") + Vector2.left * Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Mouse0))
            CatchPressed?.Invoke();

        if (Input.GetKeyUp(KeyCode.Mouse0))
            CatchReleased?.Invoke();
    }
}

