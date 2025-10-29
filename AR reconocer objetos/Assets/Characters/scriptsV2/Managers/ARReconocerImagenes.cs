using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace ARTapies_prototype.Managers
{

    [System.Serializable]
    public class RastreoPrefabs : UnityEvent<GameObject, string> { }

    [RequireComponent(typeof(ARTrackedImageManager))]
    public class ARReconocerImagenes : MonoBehaviour
    {
        private ARTrackedImageManager trackedImageManager;
        [System.Serializable]
        public struct MapeoImagenes
        {
            [Tooltip("nombre Exacto Imgen")]
            public string nombreImagen;

            [Tooltip("prefab de la imagen")]
            public GameObject prefabDesplegado;

            [Tooltip("Evento a disparar cuando reciba una imagen")]
            public RastreoPrefabs imagenDetectada;
        }

        public List<MapeoImagenes> mapeoImagen = new List<MapeoImagenes>();
        private Dictionary<string, MapeoImagenes> diccionarioMapeo = new Dictionary<string, MapeoImagenes>();
        private Dictionary<string, GameObject> prefabGenerado = new Dictionary<string, GameObject>();

        private void Awake()
        {
            trackedImageManager = GetComponent<ARTrackedImageManager>();

            foreach (var mapeo in mapeoImagen)
            {
                if (!diccionarioMapeo.ContainsKey(mapeo.nombreImagen))
                {
                    diccionarioMapeo.Add(mapeo.nombreImagen, mapeo);
                }
            }
        }

        private void OnEnable()
        {
            if (trackedImageManager != null)
            {
                trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
            }
        }

        private void OnDisable()
        {
            if (trackedImageManager != null)
            {
                trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
            }
        }

        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (var imagenIdentificada in eventArgs.added)
            {
                HandleImageAdded(imagenIdentificada);
            }

            foreach (var imagenIdentificada in eventArgs.updated)
            {
                HandleImageUpdated(imagenIdentificada);
            }
        }

        private void HandleImageAdded(ARTrackedImage imagenIdentificada)
        {
            string nombreImagen = imagenIdentificada.referenceImage.name;

            if (!diccionarioMapeo.TryGetValue(nombreImagen, out MapeoImagenes mapeo) || prefabGenerado.ContainsKey(nombreImagen))
            { return; }

            GameObject objeto = Instantiate(mapeo.prefabDesplegado);
            prefabGenerado.Add(nombreImagen, objeto);

            objeto.transform.SetPositionAndRotation(
                imagenIdentificada.transform.position,
                imagenIdentificada.transform.rotation);

            mapeo.imagenDetectada?.Invoke(objeto, nombreImagen);
        }

        private void HandleImageUpdated(ARTrackedImage imagenIdentificada)
        {
            string nombreImagen = imagenIdentificada.referenceImage.name;

            if (prefabGenerado.TryGetValue(nombreImagen, out GameObject objeto))
            {
                if (imagenIdentificada.trackingState == TrackingState.Tracking)
                {
                    objeto.transform.SetPositionAndRotation(
                        imagenIdentificada.transform.position,
                        imagenIdentificada.transform.rotation);

                    objeto.SetActive(true);
                }
                else if (imagenIdentificada.trackingState == TrackingState.Limited)
                {
                    objeto.SetActive(true);
                }
                else 
                {
                    objeto.SetActive(true);
                }
            }
        }

        public GameObject TraerModeloGenerado(string nombreImagen)
        {
            if (prefabGenerado.TryGetValue(nombreImagen, out GameObject objeto))
            {
                return objeto;
            }
            return null;
        }
    }
}