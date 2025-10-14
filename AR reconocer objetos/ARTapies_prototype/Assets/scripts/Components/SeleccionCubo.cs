using UnityEngine;
using ARTapies_prototype.Interfaces;

namespace ARTapies_prototype.Components
{
    public class SeleccionCubo : MonoBehaviour, IAtacable
    {
        private Color colorOriginal;
        private MeshRenderer meshRenderer;

        void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();

            if (meshRenderer != null)
            {
                colorOriginal = meshRenderer.material.color;
            }
        }

        public void RecibirAccion(GameObject atacante)
        {
            if (meshRenderer != null)
            {
                meshRenderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            }

            transform.position += Vector3.up * 0.05f;
        }
    }
}