using UnityEngine;
using ARTapies_prototype.Managers;

namespace ARTapies_prototype.Components
{
    public class ComportamientoCubo : MonoBehaviour
    {
        public GameObject cuboAsociado;
        private GameController controladorJuego;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            controladorJuego = FindObjectOfType<GameController>();
            
            if (controladorJuego == null )
            {
                Debug.LogError("no se encuentra el controlador");
            }

            if (cuboAsociado == null )
            {
                Debug.LogError("No se encuentra el cubo");
            }
        
        }

        public void ApretarBoton()
        {
            if (controladorJuego != null && cuboAsociado != null)
            {
                controladorJuego.InteraccionCubo(cuboAsociado);
            }
        }
    }
}
