using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CardBehavior : MonoBehaviour
{
    // Método que se llama cuando la imagen se detecta, se actualiza o se pierde
    public void OnImageUpdated(ARTrackedImage trackedImage)
    {
        if (trackedImage == null) return;

        bool isTracking = trackedImage.trackingState == TrackingState.Tracking;
        gameObject.SetActive(isTracking);

        if (isTracking)
        {
            // Mantener el cubo alineado con la carta
            transform.SetPositionAndRotation(
                trackedImage.transform.position,
                trackedImage.transform.rotation
            );
        }
    }
}
