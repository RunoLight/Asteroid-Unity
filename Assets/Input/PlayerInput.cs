using System;
using Asteroid.Presentation.Abstractions;
using UnityEngine.InputSystem;

namespace Asteroid.Input
{
    public class PlayerInput : GameInputActions.IPlayerActions, IDisposable, IPlayerInput
    {
        public event Action<bool> Forward;
        public event Action<bool> ActivateLaser;
        public event Action<float> Rotate;

        private readonly GameInputActions controls;

        public PlayerInput()
        {
            controls = new GameInputActions();
            controls.Player.SetCallbacks(this);
            controls.Player.Enable();
        }

        public void Dispose()
        {
            controls.Player.Disable();
            controls?.Dispose();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Forward?.Invoke(true);
            }

            if (context.canceled)
            {
                Forward?.Invoke(false);
            }
        }

        public void OnRotate(InputAction.CallbackContext context)
        {
            var val = context.ReadValue<float>();
            Rotate?.Invoke(val);
        }

        public void OnUseLaser(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ActivateLaser?.Invoke(true);
            }

            if (context.canceled)
            {
                ActivateLaser?.Invoke(false);
            }
        }
    }
}