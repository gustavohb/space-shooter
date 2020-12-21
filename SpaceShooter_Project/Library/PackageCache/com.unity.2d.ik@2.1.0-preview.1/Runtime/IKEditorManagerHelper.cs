using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
namespace UnityEditor.Experimental.U2D.IK
{
    [DefaultExecutionOrder(-2)]
    [ExecuteInEditMode]
    [AddComponentMenu("")]
    internal class IKEditorManagerHelper : MonoBehaviour
    {
        public UnityEvent onLateUpdate = new UnityEvent();

        void Start()
        {
            if(hideFlags != HideFlags.HideAndDontSave)
                Debug.LogWarning("This is an internal IK Component. Please remove it from your GameObject", this.gameObject);
        }
        
        void LateUpdate()
        {
            if (Application.isPlaying)
                return;

            onLateUpdate.Invoke();
        }
    }
}
#endif
