using UnityEngine;
using UnityEngine.SceneManagement;

namespace TecEduFURB.VR.Sample
{
    public class MenuManager : MonoBehaviour
    {
        /// <summary>
        /// Carrega a cena informada por parâmetro.
        /// </summary>
        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }
    }
}
