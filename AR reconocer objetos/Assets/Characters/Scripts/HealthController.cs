using UnityEngine;
using System;

[RequireComponent(typeof(Attackable))]
public class HealthController : MonoBehaviour
{
    [Header("ID único para persistir (si está vacío usa el nombre del GameObject)")]
    [SerializeField] private string characterId = "";

    [Header("Config")]
    [Tooltip("Cuántos golpes recibe antes de perder (vidas = este valor)")]
    [Min(1)]
    [SerializeField] private int maxHitsToLose = 3;

    [Header("Estado (solo lectura)")]
    [SerializeField] private int currentHits = 0;

    public event Action<int, int> OnHealthChanged; // (hits, max)

    private Attackable _atk;
    private const string PREF_PREFIX = "CHAR_HP_";

    // --- Validación inmediata al editar en el Inspector ---
    private void OnValidate()
    {
        if (maxHitsToLose < 1) maxHitsToLose = 3;
        if (currentHits < 0) currentHits = 0;
    }

    private void Awake()
    {
        _atk = GetComponent<Attackable>();

        // ID estable
        if (string.IsNullOrWhiteSpace(characterId))
            characterId = gameObject.name;

        // Seguridad extra: nunca permitir max < 1
        if (maxHitsToLose < 1) maxHitsToLose = 3;
    }

    private void OnEnable()
    {
        // Validar max otra vez por si el prefab/override lo dejó en 0
        if (maxHitsToLose < 1) maxHitsToLose = 3;

        LoadHealth(); // solo carga HITS, el max es el del Inspector

        // Clamp seguro con el max ya inicializado
        currentHits = Mathf.Clamp(currentHits, 0, maxHitsToLose);

        // Notificar al UI
        Notify();

        // Reflejar derrota si ya estaba en ese estado
        if (IsDefeated())
            _atk.Defeat();
    }

    private void OnDisable() { SaveHealth(); }
    private void OnDestroy() { SaveHealth(); }

    // === API ===

    /// Aplica un golpe (resta 1 vida). Dispara Hit y, si procede, Defeat.
    public void RegisterHit()
    {
        if (IsDefeated()) return;

        _atk.ReceiveHit();
        currentHits = Mathf.Clamp(currentHits + 1, 0, maxHitsToLose);
        SaveHealth();
        Notify();

        if (IsDefeated())
            _atk.Defeat();
    }

    /// Cura (reduce golpes). amount por defecto = 1.
    public void AddLife(int amount = 1)
    {
        if (amount <= 0) return;
        if (currentHits <= 0) return;

        currentHits = Mathf.Clamp(currentHits - amount, 0, maxHitsToLose);
        SaveHealth();
        Notify();
    }

    /// Quita vidas (atajo a golpes).
    public void RemoveLife(int amount = 1)
    {
        for (int i = 0; i < amount; i++) RegisterHit();
    }

    /// Cambia el máximo de golpes para perder (vidas = max - hits).
    public void SetMaxHitsToLose(int newMax)
    {
        if (newMax < 1) newMax = 3;
        maxHitsToLose = newMax;

        currentHits = Mathf.Clamp(currentHits, 0, maxHitsToLose);
        SaveHealth();
        Notify();

        if (IsDefeated())
            _atk.Defeat();
    }

    /// Resetea a 0 golpes (vidas completas).
    public void ResetHealth(bool forceIdle = true)
    {
        if (maxHitsToLose < 1) maxHitsToLose = 3;

        currentHits = 0;
        SaveHealth();
        Notify();

        if (forceIdle)
        {
            var binder = GetComponent<AnimBinder>();
            if (binder != null) binder.ForceIdle();
        }
    }

    public bool IsDefeated() => currentHits >= maxHitsToLose;
    public int CurrentHits => currentHits;
    public int MaxHitsToLose => maxHitsToLose;
    public int LivesRemaining => Mathf.Max(0, maxHitsToLose - currentHits);

    // === Persistencia (solo hits) ===
    private void SaveHealth()
    {
        PlayerPrefs.SetInt(PREF_PREFIX + characterId, currentHits);
        PlayerPrefs.Save();
    }

    private void LoadHealth()
    {
        currentHits = PlayerPrefs.GetInt(PREF_PREFIX + characterId, 0);
    }

    private void Notify() => OnHealthChanged?.Invoke(currentHits, maxHitsToLose);

    // Utilidad: victoria manual
    public void DoVictory()
    {
        if (IsDefeated()) return;
        _atk.Victory();
    }

    // === Menús de Debug en el Inspector ===
    [ContextMenu("Health/Print State")]
    private void PrintState()
    {
        Debug.Log($"[Health] {characterId} -> hits={currentHits}, max={maxHitsToLose}, lives={LivesRemaining}");
    }

    [ContextMenu("Health/Clear Persisted State")]
    private void ClearPersisted()
    {
        PlayerPrefs.DeleteKey(PREF_PREFIX + characterId);
        PlayerPrefs.Save();
        Debug.Log($"[Health] Cleared persisted hits for {characterId}");
    }

    [ContextMenu("Health/Force Max=3")]
    private void ForceMax3()
    {
        maxHitsToLose = 3;
        currentHits = Mathf.Clamp(currentHits, 0, maxHitsToLose);
        Notify();
        Debug.Log($"[Health] Forced max to 3 for {characterId}");
    }
}
