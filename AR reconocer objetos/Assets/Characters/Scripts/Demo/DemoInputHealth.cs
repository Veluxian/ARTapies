using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(HealthController))]
public class DemoInputHealth : MonoBehaviour
{
    private HealthController _hp;

    void Awake() => _hp = GetComponent<HealthController>();

    void Update()
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            if (Keyboard.current.jKey.wasPressedThisFrame) _hp.DoAttack();   // Ataca
            if (Keyboard.current.kKey.wasPressedThisFrame) _hp.RegisterHit(); // Recibe daño (cuenta golpe)
            if (Keyboard.current.vKey.wasPressedThisFrame) _hp.DoVictory();   // Victoria manual
            if (Keyboard.current.bKey.wasPressedThisFrame) _hp.ResetHealth(); // Reset vida
        }
#else
        if (Input.GetKeyDown(KeyCode.J)) _hp.DoAttack();
        if (Input.GetKeyDown(KeyCode.K)) _hp.RegisterHit();
        if (Input.GetKeyDown(KeyCode.V)) _hp.DoVictory();
        if (Input.GetKeyDown(KeyCode.B)) _hp.ResetHealth();
#endif
    }
}
