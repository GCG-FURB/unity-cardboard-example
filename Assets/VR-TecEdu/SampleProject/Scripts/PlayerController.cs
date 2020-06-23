using UnityEngine;

namespace TecEduFURB.VR.Sample
{
    /// <summary>
    /// Responsável por controlar a movimentação do Player.
    /// Originalmente utilizado pelo prefab Player.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float maxDistance = 3;
        [SerializeField] private float speed = 2;

        private Vector3 target;

        void Start()
        {
            // Define o target como sendo x unidades a direita (controlavel por maxDistance).
            target = new Vector3(transform.position.x + maxDistance, transform.position.y, transform.position.z);
        }

        void Update()
        {
            UpdatePlayerPosition();
        }

        /// <summary>
        /// Atualiza a posição do player sempre movendo ele em direção ao target (que começa a direita do jogador).
        /// Quando o player alcança o target, inverte a posição do target (se estava na direita, vai para a esquerda e vice versa).
        /// </summary>
        private void UpdatePlayerPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.001f)
                InvertTargetPosition();
        }

        /// <summary>
        /// Inverte a posição do target no eixo X.
        /// </summary>
        private void InvertTargetPosition()
        {
            target = new Vector3(target.x * -1, target.y, target.z);
        }
    }
}
