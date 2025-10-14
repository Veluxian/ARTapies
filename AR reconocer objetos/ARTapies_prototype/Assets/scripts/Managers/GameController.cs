using UnityEngine;
//using ARTapies_prototype.Interfaces;
using ARTapies_prototype.Entities;

namespace ARTapies_prototype.Managers
{
    public class GameController : MonoBehaviour
    {
        public GameObject prefabProyectil;
        private GameObject cuboSeleccionado = null;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

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
        private void AccionAtacar(GameObject atacante, GameObject objetivo)
        {
            //IAtacable objetivoAtacable = objetivo.GetComponent<IAtacable>();

            //if (objetivoAtacable != null)
            //{
            //    objetivoAtacable.RecibirAccion(atacante);
            //}
            if (prefabProyectil != null)
            {
                GameObject proyectilGo = Instantiate(prefabProyectil);
                Proyectil proyectilScript = proyectilGo.GetComponent<Proyectil>();

                if (proyectilScript != null)
                {
                    proyectilScript.Inicializar(atacante, objetivo);
                    atacante.transform.Rotate(0, 45f, 0);
                }
            }
            else
            {
                Debug.LogError("el Prefab no funciona");
            }
        }
    }
}