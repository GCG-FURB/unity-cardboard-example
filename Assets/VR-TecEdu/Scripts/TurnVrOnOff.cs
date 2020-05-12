using UnityEngine;
using UnityEngine.XR;

public class TurnVrOnOff : MonoBehaviour {

    [SerializeField] private bool vrModeEnabled = false;

    void Start() {
        XRSettings.enabled = vrModeEnabled;    
    }

}
