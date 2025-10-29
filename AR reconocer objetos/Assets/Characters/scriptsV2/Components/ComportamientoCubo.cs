using UnityEngine;
using System;

namespace ARTapies_prototype.Components
{
    public class ComportamientoCubo : MonoBehaviour
    {
        public static event Action<GameObject> OnCuboSeleccionado;
        public GameObject cuboAsociado;
        public void SetCuboAsociado(GameObject nuevoModelo, string nombreImagen)
        {
            cuboAsociado = nuevoModelo;
        }

        public void ApretarBoton()
        {
            if (cuboAsociado != null)
            {
                OnCuboSeleccionado?.Invoke(cuboAsociado);
            }
        }
    }
}