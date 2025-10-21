using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem; // Nuevo Input System
#endif

[RequireComponent(typeof(Attackable))]
public class DemoInput : MonoBehaviour
{
    private Attackable _atk;

    void Awake() => _atk = GetComponent<Attackable>();

    void Update()
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            if (Keyboard.current.jKey.wasPressedThisFrame) _atk.Attack();      // J
            if (Keyboard.current.kKey.wasPressedThisFrame) _atk.ReceiveHit();  // K
            if (Keyboard.current.vKey.wasPressedThisFrame) _atk.Victory();     // V
            if (Keyboard.current.bKey.wasPressedThisFrame) _atk.Defeat();      // B
        }
#else
        if (Input.GetKeyDown(KeyCode.J)) _atk.Attack();
        if (Input.GetKeyDown(KeyCode.K)) _atk.ReceiveHit();
        if (Input.GetKeyDown(KeyCode.V)) _atk.Victory();
        if (Input.GetKeyDown(KeyCode.B)) _atk.Defeat();
#endif
    }
}
