using UnityEngine;

/// <summary>
/// Adjunta el prefab LifeCounter a este personaje.
/// </summary>
[RequireComponent(typeof(HealthController))]
public class HealthUIBinder : MonoBehaviour
{
    [Header("Prefab del contador (UI)")]
    public GameObject lifeCounterPrefab;

    [Header("Canvas donde se colocará")]
    public Canvas targetCanvas;

    [Header("Altura sobre el personaje")]
    public float heightOffset = 2.0f;

    private GameObject _instance;

    void Start()
    {
        if (lifeCounterPrefab == null)
        {
            Debug.LogError($"[{name}] Falta asignar el prefab LifeCounter.");
            return;
        }

        if (targetCanvas == null)
        {
            targetCanvas = FindObjectOfType<Canvas>();
            if (targetCanvas == null)
            {
                Debug.LogError($"[{name}] No encontré Canvas en la escena.");
                return;
            }
        }

        _instance = Instantiate(lifeCounterPrefab, targetCanvas.transform);
        var ui = _instance.GetComponent<LifeCounterUI>();

        if (ui != null)
        {
            ui.target = transform; // este personaje
            ui.health = GetComponent<HealthController>();
            ui.heightOffset = heightOffset;
        }
    }

    void OnDestroy()
    {
        if (_instance != null)
            Destroy(_instance);
    }
}
