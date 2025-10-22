using UnityEngine;

/// Controla vida por "golpes" y persiste el estado.
/// Por defecto: pierde al llegar a 3 golpes (puedes cambiarlo abajo).
[RequireComponent(typeof(Attackable))]
public class HealthController : MonoBehaviour
{
    [Header("ID único del personaje para persistir (por defecto usa el nombre del GameObject)")]
    [SerializeField] private string characterId = "";

    [Header("Config")]
    [Tooltip("Cuántos golpes recibe antes de perder")]
    [SerializeField] private int maxHitsToLose = 3; // <-- CAMBIA AQUÍ si quieres 5, 10, etc.

    [Header("Estado (solo lectura)")]
    [SerializeField] private int currentHits = 0;

    private Attackable _atk;
    private const string PREF_PREFIX = "CHAR_HP_";

    void Awake()
    {
        _atk = GetComponent<Attackable>();
        if (string.IsNullOrWhiteSpace(characterId))
            characterId = gameObject.name; // ID por defecto
    }

    void OnEnable()
    {
        LoadHealth();
        // Si ya estaba derrotado cuando se volvió a habilitar (ej: AR pierde tracking y vuelve)
        if (IsDefeated())
            _atk.Defeat();
    }

    void OnDisable() { SaveHealth(); }
    void OnDestroy() { SaveHealth(); }

    // Llama esto cuando reciba daño:
    public void RegisterHit()
    {
        if (IsDefeated()) return;

        _atk.ReceiveHit();        // dispara animación "Hit"
        currentHits++;
        SaveHealth();

        if (IsDefeated())
        {
            _atk.Defeat();        // dispara animación "Defeat"
            // Si NO quieres que vuelva a Idle, elimina la transición Defeat->Idle en el Animator.
        }
    }

    // Por si quieres registrar un "ataque" (animación de ataque propia)
    public void DoAttack() => _atk.Attack();

    // Para victoria manual
    public void DoVictory()
    {
        if (IsDefeated()) return;
        _atk.Victory();
    }

    public bool IsDefeated() => currentHits >= maxHitsToLose;

    public int CurrentHits => currentHits;
    public int MaxHitsToLose => maxHitsToLose;

    // === Persistencia simple con PlayerPrefs ===
    private void SaveHealth()
    {
        PlayerPrefs.SetInt(PREF_PREFIX + characterId, currentHits);
        PlayerPrefs.Save();
    }

    private void LoadHealth()
    {
        currentHits = PlayerPrefs.GetInt(PREF_PREFIX + characterId, 0);
    }

    // === Utilidades ===

    /// Resetea la vida a 0 (usa esto para reiniciar ronda/partida)
    public void ResetHealth(bool forceIdle = true)
    {
        currentHits = 0;
        SaveHealth();
        if (forceIdle)
        {
            // Si quieres, forza Idle para “reset visual”
            var binder = GetComponent<AnimBinder>();
            if (binder != null) binder.ForceIdle();
        }
    }

    /// Cambia el límite de golpes en tiempo de ejecución (por si quieres parametrizar por nivel)
    public void SetMaxHitsToLose(int newMax)
    {
        maxHitsToLose = Mathf.Max(1, newMax);
        // Si al bajar el límite ya está derrotado, reflejarlo
        if (IsDefeated()) _atk.Defeat();
        SaveHealth();
    }
}
