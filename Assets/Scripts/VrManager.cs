using UnityEngine;
using UnityEngine.XR;

public class VrManager : MonoBehaviour {

    [SerializeField] private bool vrModeEnabled;

    void Start() {
        XRSettings.enabled = vrModeEnabled;    
    }

}
