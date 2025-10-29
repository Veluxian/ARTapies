using UnityEngine;
using ARTapies_prototype.Entities;
using ARTapies_prototype.Components;

namespace ARTapies_prototype.Managers
{
    public class GameController : MonoBehaviour
    {
        public GameObject prefabProyectil;
        private GameObject cuboSeleccionado = null;
        private void OnEnable()
        {
            ComportamientoCubo.OnCuboSeleccionado += InteraccionCubo;
        }
        private void OnDisable()
        {
            ComportamientoCubo.OnCuboSeleccionado -= InteraccionCubo;
        }

        public void InteraccionCubo(GameObject cuboEscojido)
        {
            if (cuboSeleccionado == null)
            {
                cuboSeleccionado = cuboEscojido;
            }
            else
            {
                if (cuboSeleccionado == cuboEscojido)
                {
                    cuboSeleccionado = null;
                    return;
                }

                AccionAtacar(cuboSeleccionado, cuboEscojido);
                cuboSeleccionado = null;
            }
        }

        private void AnimacionAtaque(GameObject atacante)
        {
            Animator animator = atacante.GetComponent<Animator>();

            if (animator != null)
            {
                animator.SetTrigger("Ataque");
            }
        }

        private void AccionAtacar(GameObject atacante, GameObject objetivo)
        {
            AnimacionAtaque(atacante);

            SeleccionCubo atacanteCubo = atacante.GetComponent<SeleccionCubo>();

            if (prefabProyectil != null && atacanteCubo != null)
            {
                Vector3 posicionDisparo = atacanteCubo.GetSpawnPosition();
                Debug.Log($"[GameController] Intentando disparar desde: {posicionDisparo.ToString("F4")}");

                GameObject proyectilGo = Instantiate(
                    prefabProyectil,
                    posicionDisparo,
                    atacante.transform.rotation
                );

                proyectilGo.transform.SetParent(null);
                Proyectil proyectilScript = proyectilGo.GetComponent<Proyectil>();

                if (proyectilScript != null)
                {
                    proyectilScript.Inicializar(atacante, objetivo);
                }
            }
            else
            {
                if (prefabProyectil == null)
                {
                    Debug.LogError("[GameController] El Prefab Proyectil no está asignado en el Inspector.");
                }
                if (atacanteCubo == null)
                {
                    Debug.LogError($"[GameController] El atacante ({atacante.name}) NO tiene el componente SeleccionCubo.");
                }
            }
        }
    }
}