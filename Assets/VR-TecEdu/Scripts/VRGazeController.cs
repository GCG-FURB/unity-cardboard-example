using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

namespace TecEduFURB.VR
{
    /// <summary>
    /// Responsável pelo controle da seleção de objetos da cena através do reticle (ponteiro VR).
    /// É originalmente utilizado pelo prefab VRCamera, ou seja, não é necessário atribuir manualmente 
    /// este script a qualquer objeto, bastando apenas adicionar o prefab VRCamera na cena.
    /// 
    /// OBS: Para que isso funcione é necessário que a opção Virtual Reality Suported esteja ativada.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class VRGazeController : MonoBehaviour
    {
        [Tooltip("Objeto contendo o script ReticleController.")]
        [SerializeField] private ReticleController _reticleController = null;

        [Header("Configurações para Debug")]
        [SerializeField] private bool _showDebugRay = false;
        [SerializeField] private float _debugRayLength = 5f;
        [SerializeField] private float _debugRayDuration = 1f;

        private VRTargetItem _target;
        private VRTargetItem _previousTarget;
        private EventSystem _eventSystem;
        private PointerEventData _pointerEventData;

        void Start()
        {
            _eventSystem = (EventSystem)FindObjectOfType(typeof(EventSystem));

            if (_eventSystem == null)
                Debug.LogError("É necessário adicionar um objeto EventSystem na cena para que a VRCamera funcione corretamente.");

            if (Camera.allCameras.Length > 1)
                Debug.LogError("Há mais de uma camera na cena. Recomenda-se remover todas exceto a VRCamera para prevenir comportamentos incorretos.");

            _pointerEventData = new PointerEventData(_eventSystem);
        }

        void Update()
        {
            if (_eventSystem == null)
                return;

            if (_showDebugRay)
                ShowRay();

            HandleSelectionWithGazeRaycast();
        }

        /// <summary>
        /// Realiza o tratamento para selecionar objetos do tipo VRTargetItem através de Raycast.
        /// </summary>
        private void HandleSelectionWithGazeRaycast()
        {
            HandleRaycastPositioning();

            var vrTarget = FindVRTargetViaRaycast();

            if(vrTarget.gameObject)
                SetTarget(vrTarget);
            else
                _target = null;

            if (IsNewTarget())
                StartSelection();
            else if (IsPreviousTarget())
                ContinueSelection();
            else
                ResetSelection();
        }

        /// <summary>
        /// Lança um Raycast e percorre os resultados retornando o primeiro objeto colidido 
        /// que seja do tipo VRTargetItem.
        /// </summary>
        private RaycastResult FindVRTargetViaRaycast()
        {
            List<RaycastResult> results = new List<RaycastResult>();
            _eventSystem.RaycastAll(_pointerEventData, results);

            RaycastResult vrTarget = results.Find(result => IsVRTarget(result.gameObject));

            return vrTarget;
        }

        /// <summary>
        /// Atualiza o VRTarget selecionado para o RaycastResult informado por parametro.
        /// </summary>
        private void SetTarget(RaycastResult vrTarget)
        {
            _target = vrTarget.gameObject.GetComponent<VRTargetItem>();
            _reticleController.SetPosition(vrTarget);
        }

        /// <summary>
        /// Verifica se o objeto informado possui o script VRTargetItem, ou seja, é um objeto com o qual
        /// o reticle pode interagir.
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

        /// <summary>
        /// Inicia o processo de seleção de objeto, que consiste na ativação do círculo de seleção 
        /// ao redor do cursor e também a chamada do evento GazeEnter do VRTarget selecionado.
        /// </summary>
        private void StartSelection()
        {
            _reticleController.ShowRadialImage();
            _target.GazeEnter(_pointerEventData);
            if (_previousTarget)
                _previousTarget.GazeExit(_pointerEventData);
            _reticleController.StartProgress();
            _previousTarget = _target;
        }

        /// <summary>
        /// Incrementa o círculo de seleção ao redor do cursor a cada chamada.
        /// </summary>
        private void ContinueSelection()
        {
            _reticleController.ProgressRadialImage();
            if (_reticleController.IsRadialImageFilled())
                CompleteSelection();
        }

        /// <summary>
        /// Finaliza o processo de seleção de objeto, que consiste na desativação do círculo de seleção 
        /// ao redor do cursor e também a chamada do evento GazeExit do VRTarget selecionado.
        /// </summary>
        private void ResetSelection()
        {
            if (_previousTarget)
                _previousTarget.GazeExit(_pointerEventData);

            _target = null;
            _previousTarget = null;
            _reticleController.HideRadialImage();
            _reticleController.ResetProgress();
            _reticleController.SetPosition();
        }

        /// <summary>
        /// Esconde o círculo de seleção ao redor do cursor e 
        /// também chama o evento GazeComplete do VRTarget selecionado.
        /// </summary>
        private void CompleteSelection()
        {
            _reticleController.HideRadialImage();
            _target.GazeComplete(_pointerEventData);
        }

        /// <summary>
        /// Desenha um segmento de reta azul indicando o trajeto realizado por um Raycast.
        /// </summary>
        private void ShowRay()
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * _debugRayLength, Color.blue, _debugRayDuration);
        }

        /// <summary>
        /// Trata o posicionamento do Raycast gerado pelo ponteiro dependendo da plataforma utilizada.
        /// </summary>
        private void HandleRaycastPositioning()
        {
#if UNITY_EDITOR
            _pointerEventData.position = new Vector2(Screen.width / 2, Screen.height / 2);
#elif UNITY_IOS || UNITY_ANDROID
            _pointerEventData.position = new Vector2(XRSettings.eyeTextureWidth / 2, XRSettings.eyeTextureHeight / 2);
#endif
        }
    }
}
