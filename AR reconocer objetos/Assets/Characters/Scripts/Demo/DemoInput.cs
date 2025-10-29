using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem; // Nuevo Input System
#endif

[RequireComponent(typeof(Attackable))]
[RequireComponent(typeof(HealthController))]
public class DemoInput : MonoBehaviour
{
    [Header("Habilitar grupos de teclas")]
    public bool enableAnimatorKeys = true; // J/K/V/B
    public bool enableHealthKeys = true; // H/L/R/5/3

    private Attackable _atk;
    private HealthController _hp;

    void Awake()
    {
        _atk = GetComponent<Attackable>();
        _hp = GetComponent<HealthController>();
        if (_atk == null) Debug.LogError("[DemoInput] Falta Attackable.");
        if (_hp == null) Debug.LogError("[DemoInput] Falta HealthController.");
    }

    void Update()
    {
#if ENABLE_INPUT_SYSTEM
        var kb = Keyboard.current;
        if (kb == null) return;

        if (enableAnimatorKeys)
        {
            if (kb.jKey.wasPressedThisFrame) { _atk.Attack();      Debug.Log("[DemoInput] J → Attack()"); }
            if (kb.kKey.wasPressedThisFrame) { _atk.ReceiveHit();  Debug.Log("[DemoInput] K → ReceiveHit()"); }
            if (kb.vKey.wasPressedThisFrame) { _atk.Victory();     Debug.Log("[DemoInput] V → Victory()"); }
            if (kb.bKey.wasPressedThisFrame) { _atk.Defeat();      Debug.Log("[DemoInput] B → Defeat()"); }
        }

        if (enableHealthKeys)
        {
            if (kb.hKey.wasPressedThisFrame) { _hp.RegisterHit();         Debug.Log(StateLog("H → RegisterHit")); }
            if (kb.lKey.wasPressedThisFrame) { _hp.AddLife(1);            Debug.Log(StateLog("L → AddLife(+1)")); }
            if (kb.rKey.wasPressedThisFrame) { _hp.ResetHealth();         Debug.Log(StateLog("R → ResetHealth")); }
            if (kb.digit5Key.wasPressedThisFrame) { _hp.SetMaxHitsToLose(5); Debug.Log(StateLog("5 → Max=5")); }
            if (kb.digit3Key.wasPressedThisFrame) { _hp.SetMaxHitsToLose(3); Debug.Log(StateLog("3 → Max=3")); }
        }
#else
        if (enableAnimatorKeys)
        {
            if (Input.GetKeyDown(KeyCode.J)) { _atk.Attack(); Debug.Log("[DemoInput] J → Attack()"); }
            if (Input.GetKeyDown(KeyCode.K)) { _atk.ReceiveHit(); Debug.Log("[DemoInput] K → ReceiveHit()"); }
            if (Input.GetKeyDown(KeyCode.V)) { _atk.Victory(); Debug.Log("[DemoInput] V → Victory()"); }
            if (Input.GetKeyDown(KeyCode.B)) { _atk.Defeat(); Debug.Log("[DemoInput] B → Defeat()"); }
        }

        if (enableHealthKeys)
        {
            if (Input.GetKeyDown(KeyCode.H)) { _hp.RegisterHit(); Debug.Log(StateLog("H → RegisterHit")); }
            if (Input.GetKeyDown(KeyCode.L)) { _hp.AddLife(1); Debug.Log(StateLog("L → AddLife(+1)")); }
            if (Input.GetKeyDown(KeyCode.R)) { _hp.ResetHealth(); Debug.Log(StateLog("R → ResetHealth")); }
            if (Input.GetKeyDown(KeyCode.Alpha5)) { _hp.SetMaxHitsToLose(5); Debug.Log(StateLog("5 → Max=5")); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { _hp.SetMaxHitsToLose(3); Debug.Log(StateLog("3 → Max=3")); }
        }
#endif
    }

    private string StateLog(string action)
    {
        return $"[DemoInput] {action} | hits={_hp.CurrentHits}, max={_hp.MaxHitsToLose}, lives={_hp.LivesRemaining}";
    }
}
