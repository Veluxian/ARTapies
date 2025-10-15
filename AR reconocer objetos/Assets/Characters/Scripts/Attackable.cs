using UnityEngine;

[RequireComponent(typeof(AnimBinder))]
public class Attackable : MonoBehaviour
{
    private AnimBinder _binder;

    void Awake()
    {
        _binder = GetComponent<AnimBinder>();
    }

    // Llamar desde tu lógica de combate
    public void Attack()
    {
        _binder.PlayAttack();
    }

    public void ReceiveHit()
    {
        _binder.PlayHit();
    }
}
