using UnityEngine;
using ARTapies_prototype.Components; // Para SeleccionCubo si lo usas
using ARTapies_prototype.Entities;   // Si lo requieres en tu proyecto

namespace ARTapies_prototype.Managers
{
    public class VictoryController : MonoBehaviour
    {
        private GameObject cuboSeleccionado = null;

        private void OnEnable()
        {
            ComportamientoCubo.OnCuboSeleccionado += InteraccionCubo;
        }

        private void OnDisable()
        {
            ComportamientoCubo.OnCuboSeleccionado -= InteraccionCubo;
        }

        private void InteraccionCubo(GameObject cuboEscogido)
        {
            if (cuboSeleccionado == null)
            {
                cuboSeleccionado = cuboEscogido;
                return;
            }

            if (cuboSeleccionado == cuboEscogido)
            {
                cuboSeleccionado = null;
                return;
            }

            // "atacante" = primero seleccionado
            AnimacionVictoria(cuboSeleccionado);

            cuboSeleccionado = null;
        }

        private void AnimacionVictoria(GameObject objetivo)
        {
            var animator = objetivo.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Victoria");
            }
            else
            {
                Debug.LogWarning($"[VictoryController] {objetivo.name} no tiene Animator.");
            }
        }
    }
}
