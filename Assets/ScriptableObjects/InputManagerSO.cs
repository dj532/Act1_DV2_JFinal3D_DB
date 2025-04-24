using Unity.VisualScripting;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputManager")]
public class InputManagerSO : ScriptableObject
{
    Controls misControles;
    public event Action OnSaltar;
    public event Action<Vector2> OnMover;
    public event Action OnDisparar;
    public event Action OnRecargar;
    private void OnEnable()
    {
        misControles = new Controls();
        misControles.Gameplay.Enable();
        misControles.Gameplay.Saltar.started += Saltar;
        misControles.Gameplay.Disparar.started += Disparar;
        misControles.Gameplay.Recargar.started += Recargar;
        misControles.Gameplay.Mover.performed += Mover;
        misControles.Gameplay.Mover.canceled += Mover;
        
    }

    private void Recargar(InputAction.CallbackContext obj)
    {
        OnRecargar?.Invoke();
    }

    private void Disparar(InputAction.CallbackContext obj)
    {
        OnDisparar?.Invoke();
    }

    private void Mover(InputAction.CallbackContext ctx)
    {
        OnMover?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void Saltar(InputAction.CallbackContext ctx)
    {
        OnSaltar?.Invoke();
    }
}
