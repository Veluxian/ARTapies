using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem; // Nuevo Input System
#endif

[RequireComponent(typeof(Attackable))]
public class DemoInput : MonoBehaviour
{
    private Attackable _atk;

    void Awake()
    {
        _atk = GetComponent<Attackable>();
        if (_atk == null)
            Debug.LogError("[DemoInput] Falta componente Attackable en el mismo objeto.");
    }

    void Update()
    {
        // --- Nuevo Input System ---
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            if (Keyboard.current.jKey.wasPressedThisFrame)
            {
                Debug.Log("[DemoInput] J pressed → Attack()");
                _atk.Attack();
            }
            if (Keyboard.current.kKey.wasPressedThisFrame)
            {
                Debug.Log("[DemoInput] K pressed → ReceiveHit()");
                _atk.ReceiveHit();
            }
        }
#else
        // --- Input clásico ---
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("[DemoInput] J pressed → Attack()");
            _atk.Attack();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("[DemoInput] K pressed → ReceiveHit()");
            _atk.ReceiveHit();
        }
#endif
    }
}
