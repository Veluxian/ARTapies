using UnityEngine;
using ARTapies_prototype.Interfaces;

namespace ARTapies_prototype.Entities
{
    public class Proyectil : MonoBehaviour
    {
        private GameObject objetivo;
        private GameObject atacante;
        private float velocidad = 0.09f;

        public void Inicializar(GameObject atacante, GameObject objetivo)
        {
            this.atacante = atacante;
            this.objetivo = objetivo;

            Vector3 direccionInicial = (objetivo.transform.position - atacante.transform.position).normalized;
            transform.position += direccionInicial * 0.05f;
        }

        void Update()
        {
            if (objetivo == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 direccion = (objetivo.transform.position - transform.position).normalized;

            Debug.Log($"[Proyectil] Objetivo Pos: {objetivo.transform.position.ToString("F2")} | Dirección: {direccion.ToString("F2")}");

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

            if (objetivoAtacable != null)
            {
                objetivoAtacable.RecibirAccion(atacante);
            }
        }
    }
}