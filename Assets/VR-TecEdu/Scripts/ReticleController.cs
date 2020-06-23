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
    /// 
    /// OBS: Para que isso funcione é necessário que a opção Virtual Reality Suported esteja ativada.
    /// </summary>
    public class ReticleController : MonoBehaviour
    {
        [Tooltip("Imagem que será preenchida ao longo do tempo quando o reticle estiver sobre um objeto para indicar o tempo de seleção.")]
        [SerializeField] private Image _radialImage = null;
        [Tooltip("Imagem utilizada para representar o reticle.")]
        [SerializeField] private Transform _reticleTransform = null;
        [Tooltip("Distância padrão entre o reticle e câmera.")]
        [SerializeField] private float _defaultDistance = 5f;
        [Tooltip("Tempo de seleção de um objeto em segundos. Ou seja, o tempo que a radialImage levará para ser preenchida.")]
        [SerializeField] private float _selectionDuration = 0.2f;
        
        private bool _isRadialFilled = false;
        private float _selectionTimer;
        private Vector3 _originalScale;

        private void Awake()
        {
            _originalScale = _reticleTransform.localScale;
        }

        /// <summary>
        /// Define a posição padrão do reticle, isto é, seta a posição do reticle para 
        /// o centro da tela (centro da camera) na distancia padrão (_defaultDistance),
        /// atualizando também sua escala com base na distancia padrão.
        /// </summary>
        public void SetPosition()
        {
            _reticleTransform.position = Camera.main.transform.position + Camera.main.transform.forward * _defaultDistance;
            _reticleTransform.localScale = _originalScale * _defaultDistance;
        }

        /// <summary>
        /// Atualiza a posição do reticle no eixo Z para a mesma que o objeto informado,
        /// atualizando também sua escala com base na distancia do objeto informado.
        /// </summary>
        public void SetPosition(RaycastResult hit)
        {
            _reticleTransform.localPosition = new Vector3(0f, 0f, hit.distance);
            _reticleTransform.localScale = _originalScale * hit.distance;
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

        /// <summary>
        /// Aumenta gradativamente o preenchimento do círculo de seleção ao redor do reticle.
        /// </summary>
        public void ProgressRadialImage()
        {
            if (!IsRadialImageFilled())
            {
                _selectionTimer += Time.deltaTime;
                _radialImage.fillAmount = _selectionTimer / _selectionDuration;

                if (_selectionTimer >= _selectionDuration)
                {
                    _isRadialFilled = true;
                    ResetProgress();
                }
            }
        }

        /// <summary>
        /// Verifica se o círculo de seleção ao redor do reticle está totalmente preenchido,
        /// retornando true caso esteja e false caso contrário.
        /// </summary>
        public bool IsRadialImageFilled()
        {
            return _isRadialFilled;
        }

        /// <summary>
        /// Reinicia o círculo de seleção ao redor do reticle.
        /// </summary>
        public void ResetProgress()
        {
            _selectionTimer = 0f;
            _radialImage.fillAmount = 0f;
        }
    }
}
