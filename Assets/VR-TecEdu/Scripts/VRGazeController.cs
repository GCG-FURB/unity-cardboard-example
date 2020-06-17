using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TecEduFURB.VR
{
    // TODO: renomear reticle para gaze
    /// <summary>
    /// 
    /// É originalmente utilizado pelo prefab VRCamera, ou seja, não é necessário atribuir manualmente este script a 
    /// qualquer objeto, bastando apenas adicionar o prefab VRCamera na cena.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class VRGazeController : MonoBehaviour
    {
        [SerializeField] private RadialReticle _reticle = null;     // The reticle, if applicable.
        [SerializeField] private bool _showDebugRay = false;        // Optionally show the debug ray.
        [SerializeField] private float _debugRayLength = 5f;        // Debug ray length.
        [SerializeField] private float _debugRayDuration = 1f;      // How long the Debug ray will remain visible.

        // variables for EventSystem.RaycastAll
        private PointerEventData _pointerEventData;
        private EventSystem _eventSystem;

        private RaycastResult _currentTarget;
        private VRTargetItem _target;
        private VRTargetItem _previousTarget;

        void Start()
        {
            _eventSystem = (EventSystem)FindObjectOfType(typeof(EventSystem));

            if (_eventSystem == null)
                Debug.LogError("É necessário adicionar um objeto EventSystem na cena para que a VRCamera funcione corretamente.");

            if (Camera.allCameras.Length > 1)
                Debug.LogError("Há mais de uma camera na cena. Recomenda-se remover todas exceto a VRCamera para prevenir comportamentos incorretos.");
        }

        void Update()
        {
            if (_eventSystem == null)
                return;

            GazeRaycast();
        }

        private void GazeRaycast()
        {
            // Show the debug ray
            if (_showDebugRay)
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * _debugRayLength, Color.blue, _debugRayDuration);

            // Set up PointerEventData
            _pointerEventData = new PointerEventData(_eventSystem);

            // handle positioning for gaze raycast depending on platform or using Unity editor
#if UNITY_EDITOR
            _pointerEventData.position = new Vector2(Screen.width / 2, Screen.height / 2);
#elif UNITY_IOS || UNITY_ANDROID
            _pointerEventData.position = new Vector2(UnityEngine.XR.XRSettings.eyeTextureWidth / 2, UnityEngine.XR.XRSettings.eyeTextureHeight / 2);
#endif

            List<RaycastResult> results = new List<RaycastResult>();
            _eventSystem.RaycastAll(_pointerEventData, results);

            foreach (var hit in results)
            {
                if (IsVRTarget(hit.gameObject))
                {
                    _target = hit.gameObject.GetComponent<VRTargetItem>();
                    _currentTarget = hit;
                    _reticle.SetPosition(hit);

                    break;
                }

                // no targets found
                _target = null;
            }

            // If current interactive item is not the same as the last interactive item, then call GazeEnter and start fill
            if (IsNewTarget())
            {
                _reticle.ShowRadialImage();
                _target.GazeEnter(_pointerEventData);
                if (_previousTarget)
                    _previousTarget.GazeExit(_pointerEventData);
                _reticle.StartProgress();
                _previousTarget = _target;
            }
            else if (IsPreviousTarget()) // hovering over same item, advance fill progress
            {
                if (_reticle.ProgressRadialImage())         // returns true if selection is completed
                    CompleteSelection();
            }
            else
            {
                // no target hit
                if (_previousTarget)
                    _previousTarget.GazeExit(_pointerEventData);

                _target = null;
                _previousTarget = null;
                _reticle.HideRadialImage();
                _reticle.ResetProgress();
                _reticle.SetPosition();
            }
        }

        /// <summary>
        /// Verifica se o objeto informado possui o script VRTargetItem, ou seja, é um objeto com o qual
        /// o gaze pdoe interagir.
        /// </summary>
        private bool IsVRTarget(GameObject obj)
        {
            return obj.GetComponent<VRTargetItem>();
        }

        /// <summary>
        /// Verifica se o alvo atual mudou.
        /// </summary>
        private bool IsNewTarget()
        {
            return _target && _target != _previousTarget;
        }

        /// <summary>
        /// Verifica se o alvo atual ainda é o mesmo.
        /// </summary>
        private bool IsPreviousTarget()
        {
            return _target && _target == _previousTarget;
        }

        private void CompleteSelection()
        {
            // hide radial image
            _reticle.HideRadialImage();

            // radial progress completed, call completion events on target
            _target.GazeComplete(_pointerEventData);
        }
    }
}
