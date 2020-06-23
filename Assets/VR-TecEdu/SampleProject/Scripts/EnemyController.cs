using UnityEngine;

namespace TecEduFURB.VR.Sample
{
    /// <summary>
    /// Responsável por controlar o comportamento do Enemy.
    /// Originalmente utilizado pelo prefab Enemy.
    /// </summary>
    public class EnemyController : MonoBehaviour
    {
        private Material material;

        void Start()
        {
            material = GetComponent<Renderer>().material;
        }

        /// <summary>
        /// Atualiza a cor deste objeto. 
        /// Se a cor atual for azul troca para vermelha, se for vermelha troca para azul. 
        /// </summary>
        public void ChangeColor_OnClick()
        {
            if (material.color == Color.blue)
                material.SetColor("_Color", Color.red);
            else
                material.SetColor("_Color", Color.blue);
        }
    }


}
