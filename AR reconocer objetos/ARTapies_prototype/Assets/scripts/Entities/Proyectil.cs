using UnityEngine;
using ARTapies_prototype.Interfaces;

namespace ARTapies_prototype.Entities
{
    public class Proyectil : MonoBehaviour
    {
        private GameObject objetivo;
        private GameObject atacante;
        private float velocidad = 5f;

        public void Inicializar(GameObject atacante, GameObject objetivo)
        {
            this.atacante = atacante;
            this.objetivo = objetivo;

            transform.position = atacante.transform.position;
        }
        void Update()
        {
            if (objetivo == null) 
            {
                Destroy(gameObject);
                return;
            }

            Vector3 direccion = (objetivo.transform.position - transform.position).normalized;
            transform.position += direccion * velocidad * Time.deltaTime;

            if (Vector3.Distance(transform.position, objetivo.transform.position) < 0.1f)
            {
                Impactar();
                Destroy(gameObject);
            }
        }

        private void Impactar()
        {
            IAtacable objetivoAtacable = objetivo.GetComponent<IAtacable>();

            if(objetivoAtacable != null )
            {
                objetivoAtacable.RecibirAccion(atacante);
            }
        }
    }
}