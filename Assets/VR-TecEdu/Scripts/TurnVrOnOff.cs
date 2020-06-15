using UnityEngine;
using UnityEngine.XR;

namespace TecEduFURB.VR
{
    public class TurnVrOnOff : MonoBehaviour
    {
        [SerializeField] private bool vrModeEnabled = false;

        void Start()
        {
            XRSettings.enabled = vrModeEnabled;
        }
    }
}
