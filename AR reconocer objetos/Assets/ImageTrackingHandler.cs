using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTrackingHandler : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        if (trackedImageManager == null)
            Debug.LogError("ImageTrackingHandler: no se encontró ARTrackedImageManager en este GameObject.");
    }

    private void OnEnable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackablesChanged.AddListener(OnTrackablesChanged);
    }

    private void OnDisable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackablesChanged.RemoveListener(OnTrackablesChanged);
    }

    private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        // Imágenes añadidas
        foreach (ARTrackedImage trackedImage in eventArgs.added)
            NotifyCardBehavior(trackedImage);

        // Imágenes actualizadas
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
            NotifyCardBehavior(trackedImage);

        // Imágenes eliminadas (iterar sobre valores si es diccionario)
        foreach (var kvp in eventArgs.removed)
            NotifyCardBehavior(kvp.Value);
    }

    private void NotifyCardBehavior(ARTrackedImage trackedImage)
    {
        if (trackedImage == null) return;

        var card = trackedImage.GetComponentInChildren<CardBehavior>();
        if (card != null)
            card.OnImageUpdated(trackedImage);
        else
            Debug.LogWarning($"ImageTrackingHandler: no se encontró CardBehavior para la imagen '{trackedImage.referenceImage.name}'.");
    }
}
