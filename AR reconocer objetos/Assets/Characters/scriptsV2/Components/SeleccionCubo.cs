using UnityEngine;
using ARTapies_prototype.Interfaces;

namespace ARTapies_prototype.Components
{
    public class SeleccionCubo : MonoBehaviour, IAtacable
    {
        private MeshRenderer meshRenderer;
        public Transform spawnPoint;
        private Animator animator;

        void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            animator = GetComponent<Animator>();
        }

        public void RecibirAccion(GameObject atacante)
        {
            if (animator !=null)
            {
                animator.SetTrigger("Defensa");
            }
        }

        public Vector3 GetSpawnPosition()
        {
            if (spawnPoint != null)
            {
                return spawnPoint.position;
            }
            return transform.position;
        }
    }
}