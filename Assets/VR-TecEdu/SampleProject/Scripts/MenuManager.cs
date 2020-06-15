using UnityEngine;
using UnityEngine.SceneManagement;

namespace TecEduFURB.VR.Sample
{
    public class MenuManager : MonoBehaviour
    {
        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }
    }
}
