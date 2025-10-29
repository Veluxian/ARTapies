using UnityEngine;
using ARTapies_prototype.Components;
using ARTapies_prototype.Entities;

namespace ARTapies_prototype.Managers
{
    public class ItemUseController : MonoBehaviour
    {
        [Header("Opcional: efecto visual del item")]
        public GameObject prefabEfectoItem;

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

            // atacante = primero seleccionado; objetivo = segundo (si lo necesitas)
            var atacante = cuboSeleccionado;
            var objetivo = cuboEscogido;

            AnimacionUsarItem(atacante, objetivo);

            cuboSeleccionado = null;
        }

        private void AnimacionUsarItem(GameObject atacante, GameObject objetivo)
        {
            var animator = atacante.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("UsarItem");
            }
            else
            {
                Debug.LogWarning($"[ItemUseController] {atacante.name} no tiene Animator.");
            }

            // Efecto visual opcional
            if (prefabEfectoItem != null)
            {
                Vector3 spawn = atacante.transform.position;
                var sel = atacante.GetComponent<SeleccionCubo>();
                if (sel != null) spawn = sel.GetSpawnPosition();

                var fx = Instantiate(prefabEfectoItem, spawn, atacante.transform.rotation);
                fx.transform.SetParent(null);
            }
        }
    }
}
