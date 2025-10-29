using UnityEngine;
using ARTapies_prototype.Components;
using ARTapies_prototype.Entities;

namespace ARTapies_prototype.Managers
{
    public class DefeatController : MonoBehaviour
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

            // "objetivo" = segundo seleccionado
            AnimacionDerrota(cuboEscogido);

            cuboSeleccionado = null;
        }

        private void AnimacionDerrota(GameObject objetivo)
        {
            var animator = objetivo.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Derrota");
            }
            else
            {
                Debug.LogWarning($"[DefeatController] {objetivo.name} no tiene Animator.");
            }
        }
    }
}
