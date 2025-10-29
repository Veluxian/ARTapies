using UnityEngine;
using TMPro;

[RequireComponent(typeof(HealthController))]
public class FloatingHealthUI : MonoBehaviour
{
    [Header("Ajustes visuales")]
    public Vector3 worldOffset = new Vector3(0, 2.0f, 0);
    public float scale = 1.0f;
    public bool showHearts = false; // Mostrar "♥" por vida
    public bool colorizeOnZero = true; // Rojo al quedar en 0

    private HealthController _hp;
    private Camera _cam;
    private Transform _uiRoot;
    private TextMeshPro _tmp;
    private int _lastHits = -1, _lastMax = -1;

    private static readonly Color COLOR_OK = Color.white;
    private static readonly Color COLOR_ZERO = new Color(1f, 0.2f, 0.2f); // rojizo

    private void Awake()
    {
        _hp = GetComponent<HealthController>();
        _cam = Camera.main;

        // Crear nodo UI si no existe
        var existing = transform.Find("HealthUI_World");
        _uiRoot = existing ? existing : new GameObject("HealthUI_World").transform;
        if (existing == null)
        {
            _uiRoot.SetParent(transform);
            _uiRoot.localRotation = Quaternion.identity;
        }
        _uiRoot.localScale = Vector3.one * scale;

        // Crear TMP si no existe
        _tmp = _uiRoot.GetComponentInChildren<TextMeshPro>();
        if (_tmp == null)
        {
            var txtGO = new GameObject("TMP_Lives");
            txtGO.transform.SetParent(_uiRoot, false);
            _tmp = txtGO.AddComponent<TextMeshPro>();
            _tmp.alignment = TextAlignmentOptions.Center;
            _tmp.enableAutoSizing = true;
            _tmp.fontSize = 3.5f;
            _tmp.color = COLOR_OK;
            _tmp.outlineWidth = 0.2f;
            _tmp.outlineColor = Color.black;
            _tmp.text = "";
        }
    }

    private void OnEnable()
    {
        _hp.OnHealthChanged += OnHealthChanged;
        ForceRefresh();
    }

    private void OnDisable()
    {
        _hp.OnHealthChanged -= OnHealthChanged;
    }

    private void LateUpdate()
    {
        // Billboard hacia la cámara y mantener offset
        if (_cam == null) _cam = Camera.main;
        if (_cam != null)
            _uiRoot.rotation = Quaternion.LookRotation(_uiRoot.position - _cam.transform.position);

        _uiRoot.position = transform.position + worldOffset;
    }

    private void OnHealthChanged(int hits, int max)
    {
        // Fallback por si el max llegó 0 desde el Inspector
        if (max < 1) max = 3;

        if (hits == _lastHits && max == _lastMax) return;
        _lastHits = hits;
        _lastMax = max;

        int lives = Mathf.Max(0, max - hits);

        if (showHearts)
        {
            _tmp.text = lives > 0 ? new string('♥', lives) : "✖";
        }
        else
        {
            _tmp.text = $"Vidas: {lives}";
        }

        if (colorizeOnZero)
            _tmp.color = lives > 0 ? COLOR_OK : COLOR_ZERO;
    }

    private void ForceRefresh()
    {
        // Leer estado actual del HealthController
        int hits = _hp.CurrentHits;
        int max = _hp.MaxHitsToLose < 1 ? 3 : _hp.MaxHitsToLose;
        OnHealthChanged(hits, max);
    }
}
