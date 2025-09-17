using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CardBehavior : MonoBehaviour
{
    // Start se llama antes de la primera actualización del frame
    void Start()
    {
        // Se hace nada en Start, ya que la lógica principal
        // se maneja a través del ARTrackedImageManager
    }

    // Este método es llamado por el ARTrackedImageManager cuando una imagen es detectada, actualizada o perdida
    public void OnImageUpdated(ARTrackedImage arTrackedImage)
    {
        // El objeto de juego (el cubo) se activa si la imagen se está rastreando
        // Se desactiva si el estado es None o Limited
        bool isTracking = (arTrackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking);
        gameObject.SetActive(isTracking);

        if (isTracking)
        {
            Debug.Log("Image is now being tracked: " + arTrackedImage.referenceImage.name);
        }
        else
        {
            Debug.Log("Image tracking lost: " + arTrackedImage.referenceImage.name);
        }
    }
}