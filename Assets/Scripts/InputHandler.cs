using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public event Action FKeyPressed;
    public event Action RKeyPressed;
    public event Action SpaceKeyPressed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            FKeyPressed?.Invoke();

        if (Input.GetKeyDown(KeyCode.R))
            RKeyPressed?.Invoke();

        if (Input.GetKeyDown(KeyCode.Space))
            SpaceKeyPressed?.Invoke();
    }
}