using UnityEngine;
using UnityEngine.XR;

namespace TecEduFURB.VR
{
    /// <summary>
    /// Por padrão, ao ativar a opção Virtual Reality Suported em Edit -> Project Settings -> Player -> XR Settings
    /// todas as cenas iniciarão automaticamente em VR. 
    /// Esta classe serve para permitir que o projeto possua cenas em VR e cenas normais.
    /// Para isso basta criar um objeto na cena, atribuir este script a ele e marcar
    /// a variável VRModeEnabled como true ou false dependendo do que preferir.
    /// 
    /// OBS: Para que isso funcione é necessário que a opção Virtual Reality Suported esteja ativada.
    /// </summary>
    public class TurnVrOnOff : MonoBehaviour
    {
        [Header("Selecionar caso queira utilizar VR na cena")]
        [SerializeField] private bool VRModeEnabled = false;

        void Start()
        {
            XRSettings.enabled = VRModeEnabled;
        }
    }
}
