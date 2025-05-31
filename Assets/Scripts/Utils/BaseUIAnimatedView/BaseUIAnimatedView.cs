using System.Collections;
using UnityEngine;

namespace WeatherOrNot.Utils
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseUIAnimatedUIView : MonoBehaviour
    {
        [SerializeField] private float m_fadeDuration = 0.25f;
        [SerializeField] private GameObject m_rootObject;

        private CanvasGroup m_canvasGroup;
        private Coroutine m_animationRoutine;
        private bool m_isActive;

        public bool IsActive => m_isActive;

        protected virtual void Awake()
        {
            m_canvasGroup = GetComponent<CanvasGroup>();

            if (m_rootObject == null)
                m_rootObject = gameObject;
        }

        public void ToggleRootView(bool isActive)
        {
            if (m_animationRoutine != null)
            {
                StopCoroutine(m_animationRoutine);
                m_animationRoutine = null;
            }

            m_rootObject.SetActive(isActive);
            m_canvasGroup.alpha = isActive ? 1f : 0f;
            m_canvasGroup.interactable = isActive;
            m_canvasGroup.blocksRaycasts = isActive;

            m_isActive = isActive;
            OnAnimationEnd(isActive);
        }

        public void AnimatedToggleRootView(bool isActive)
        {
            if (m_animationRoutine != null)
            {
                StopCoroutine(m_animationRoutine);
            }

            m_animationRoutine = StartCoroutine(FadeRoutine(isActive));
        }

        private IEnumerator FadeRoutine(bool isActive)
        {
            if (isActive)
            {
                m_rootObject.SetActive(true);
            }

            float startAlpha = m_canvasGroup.alpha;
            float endAlpha = isActive ? 1f : 0f;
            float elapsed = 0f;

            while (elapsed < m_fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / m_fadeDuration;
                m_canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                yield return null;
            }

            m_canvasGroup.alpha = endAlpha;
            m_canvasGroup.interactable = isActive;
            m_canvasGroup.blocksRaycasts = isActive;

            m_isActive = isActive;

            if (!isActive)
            {
                m_rootObject.SetActive(false);
            }

            m_animationRoutine = null;
            OnAnimationEnd(isActive);
        }

        protected virtual void OnAnimationEnd(bool isActive)
        {
        }
    }
}