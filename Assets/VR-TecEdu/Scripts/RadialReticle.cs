using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TecEduFURB.VR
{
    /// <summary>
    /// Script originalmente criado pela Unity Technologies como parte do VRStandardAssets e modificado para permitir 
    /// interação através do reticle (ponteiro) com objetos 2D (Canvas) e 3D (GameObjects), bastando apontar o reticle 
    /// para o objeto em questão. 
    /// É originalmente utilizado pelo prefab VRCamera, ou seja, não é necessário atribuir manualmente este script a 
    /// qualquer objeto, bastando apenas adicionar o prefab VRCamera na cena.
    /// </summary>
    public class RadialReticle : MonoBehaviour
    {
        [SerializeField] private float _defaultDistance = 5f;           // default distance between reticle and camera
        [SerializeField] private bool _isNormalUsed = false;            // is reticle parallel to a surface or always facing camera.
        [SerializeField] private Transform _reticleTransform = null;    // tranbslate reticle to target object
        [SerializeField] private Transform _camera = null;              // camera position is reference for placing reticle

        [SerializeField] private Image _radialImage = null;             // image that is filled as an indication of how long the gazepointer is hovered over item
        [SerializeField] private float _radialDuration = 2f;            // time it takes to complete the reticle's fill (in seconds).
        private bool _isRadialFilled = false;                           // check whether fill is complete, to avoid repeated hover completions
        private float _timer;                                           // used to check timing of hover completion

        private Vector3 _originalScale;                                 // initial scale of reticle so it can be rescaled and scale restored
        private Quaternion _originalRotation;                           // initial rotation of reticle so it can be rotated and rotation later restored

        private void Awake()
        {
            // Store initial scale and rotation.
            _originalScale = _reticleTransform.localScale;
            _originalRotation = _reticleTransform.localRotation;
        }

        // when no targets are hit -> set default distance of reticle
        public void SetPosition()
        {
            // Set position of reticle to default distance in front of camera
            _reticleTransform.position = _camera.position + _camera.forward * _defaultDistance;

            // Set scale based on the original and distance from the camera
            _reticleTransform.localScale = _originalScale * _defaultDistance;

            // rotation is default
            _reticleTransform.localRotation = _originalRotation;
        }

        // when target is hit -> reticle is moved to target position
        public void SetPosition(RaycastResult hit)
        {
            _reticleTransform.localPosition = new Vector3(0f, 0f, hit.distance);
            _reticleTransform.localScale = _originalScale * hit.distance;

            if (_isNormalUsed)
                // set rotation based on forward vector along the normal of hit point
                _reticleTransform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.worldNormal);
            else
                // use original rotation
                _reticleTransform.localRotation = _originalRotation;
        }

        /// <summary>
        /// Torna visível o círculo de seleção que aparece ao redor do reticle para demonstrar a seleção de objetos.
        /// </summary>
        public void ShowRadialImage()
        {
            _radialImage.gameObject.SetActive(true);
        }

        /// <summary>
        /// Esconde o círculo de seleção que aparece ao redor do reticle para demonstrar a seleção de objetos.
        /// </summary>
        public void HideRadialImage()
        {
            _radialImage.gameObject.SetActive(false);
        }

        /// <summary>
        /// Faz com que o círculo de seleção do reticle possa começar a ser preenchido.
        /// </summary>
        public void StartProgress()
        {
            _isRadialFilled = false;
        }

        // TODO: tornar esse método void e criar outro para dizer se o radial foi totalmente preenchido ou nao
        /// <summary>
        /// Aumenta gradativamente o preenchimento do círculo de seleção ao redor do reticle.
        /// </summary>
        /// <returns>True quando o círculo estiver preenchido e false caso contrário.</returns>
        public bool ProgressRadialImage()
        {
            if (!_isRadialFilled)
            {
                // advance timer
                _timer += Time.deltaTime;
                _radialImage.fillAmount = _timer / _radialDuration;

                // if timer exceeds duration, complete progress and reset
                if (_timer >= _radialDuration)
                {
                    ResetProgress();
                    _isRadialFilled = true;
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Reinicia o círculo de seleção ao redor do reticle.
        /// </summary>
        public void ResetProgress()
        {
            _timer = 0f;
            _radialImage.fillAmount = 0f;
        }
    }
}
