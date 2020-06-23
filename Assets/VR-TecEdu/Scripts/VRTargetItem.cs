﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TecEduFURB.VR
{
    /// <summary>
    /// Responsável por identificar um objeto da cena como um VR Target, fazendo com que ele seja 
    /// reconhecível pelo reticle (ponteiro) da VRCamera. Ou seja, todos os objetos da cena que possam
    /// ser selecionáveis pelo reticle devem obrigatoriamente possuir este script anexado a eles.
    /// </summary>
    public class VRTargetItem : MonoBehaviour
    {
        [Header("Chamado quando o ponteiro colide com este objeto")]
        [SerializeField] private UnityEvent m_gazeEnterEvent = null;
        [Header("Chamado quando o ponteiro deixa de colidir com este objeto")]
        [SerializeField] private UnityEvent m_gazeExitEvent = null;
        [Header("Chamado quando este objeto é selecionado")]
        [SerializeField] private UnityEvent m_completionEvent = null;

        /// <summary>
        /// Chama o evento GazeEnterEvent definido neste script via Editor.
        /// </summary>
        public void GazeEnter(PointerEventData pointer)
        {
            m_gazeEnterEvent.Invoke();
        }

        /// <summary>
        /// Chama o evento GazeExitEvent definido neste script via Editor.
        /// </summary>
        public void GazeExit(PointerEventData pointer)
        {
            m_gazeExitEvent.Invoke();
        }

        /// <summary>
        /// Chama o evento CompletionEvent definido neste script via Editor.
        /// </summary>
        public void GazeComplete(PointerEventData pointer)
        {
            m_completionEvent.Invoke();
        }
    }
}
