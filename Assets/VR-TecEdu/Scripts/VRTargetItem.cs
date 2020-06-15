using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TecEduFURB.VR
{
    public class VRTargetItem : MonoBehaviour
    {
        // [Header("Invoked when gaze pointer enters object")]
        [Header("Chamado quando o ponteiro colide com este objeto")]
        [SerializeField] private UnityEvent m_gazeEnterEvent = null;
        // [Header("Invoked when gaze pointer exits object")]
        [Header("Chamado quando o ponteiro deixa de colidir com este objeto")]
        [SerializeField] private UnityEvent m_gazeExitEvent = null;
        // [Header("Invoked when this target item is selected")]
        [Header("Chamado quando este objeto é selecionado")]
        [SerializeField] private UnityEvent m_completionEvent = null;

        private Selectable m_selectable;
        private ISubmitHandler m_submit;

        private void Awake()
        {
            m_selectable = GetComponent<Selectable>();
            m_submit = GetComponent<ISubmitHandler>();
        }

        public void GazeEnter(PointerEventData pointer)
        {
            // When the user looks at the rendering of the scene, show the radial.

            if (m_selectable)
                m_selectable.OnPointerEnter(pointer);
            else
                m_gazeEnterEvent.Invoke();
        }

        public void GazeExit(PointerEventData pointer)
        {
            // When the user looks away from the rendering of the scene, hide the radial.
            if (m_selectable)
                m_selectable.OnPointerExit(pointer);
            else
                m_gazeExitEvent.Invoke();
        }

        public void GazeComplete(PointerEventData pointer)
        {
            // invoke events that are set up in the inspector
            if (m_submit != null)
                m_submit.OnSubmit(pointer);
            else
                m_completionEvent.Invoke();
        }
    }
}
