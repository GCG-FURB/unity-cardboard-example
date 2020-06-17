using UnityEngine;

namespace TecEduFURB.VR.Sample
{
    public class EnemyController : MonoBehaviour
    {
        private Material material;
        private bool blue = false;

        void Start()
        {
            material = GetComponent<Renderer>().material;
        }

        public void ChangeColor()
        {
            if (blue)
            {
                material.SetColor("_Color", Color.red);
                blue = false;
            }
            else
            {
                material.SetColor("_Color", Color.blue);
                blue = true;
            }
        }
    }
}
