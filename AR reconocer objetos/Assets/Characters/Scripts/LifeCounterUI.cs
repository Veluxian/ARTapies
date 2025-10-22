using UnityEngine;
using TMPro;

/// <summary>
/// Script del prefab LifeCounter.
/// Se coloca dentro del Canvas y sigue visualmente a un personaje.
/// </summary>
public class LifeCounterUI : MonoBehaviour
{
    [Header("Referencia al texto del prefab")]
    public TextMeshProUGUI label;

    [Header("Datos del personaje a seguir")]
    public Transform target;          // personaje
    public HealthController health;   // script de vida

    [Header("Ajustes visuales")]
    public float heightOffset = 2.0f;       // altura sobre la cabeza
    public Vector3 screenOffset = Vector3.zero;

    private RectTransform _rt;
    private Camera _cam;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        if (label == null) label = GetComponent<TextMeshProUGUI>();
        _cam = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null || label == null || _cam == null) return;

        // Calcular posición en pantalla
        Vector3 worldPos = target.position + Vector3.up * heightOffset;
        Vector3 screenPos = _cam.WorldToScreenPoint(worldPos);

        // Si está detrás de cámara, esconder
        label.enabled = screenPos.z > 0f;
        _rt.position = screenPos + screenOffset;

        // Actualizar número de vidas
        UpdateText();
    }

    private void UpdateText()
    {
        if (health == null) return;

        int vidasRestantes = Mathf.Max(0, health.MaxHitsToLose - health.CurrentHits);
        label.text = vidasRestantes.ToString();

        // Colores según cantidad
        if (vidasRestantes > 1) label.color = Color.green;
        else if (vidasRestantes == 1) label.color = Color.yellow;
        else label.color = Color.red;
    }
}
