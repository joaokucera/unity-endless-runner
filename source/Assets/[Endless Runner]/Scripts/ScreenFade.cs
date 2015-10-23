using System.Collections;
using UnityEngine;

namespace EndlessRunner
{
    [AddComponentMenu("CUSTOM / Screen Fade")]
    public class ScreenFade : Singleton<ScreenFade>
    {
        [SerializeField]
        private CanvasGroup m_canvasGroup;

        private const float FadeTime = 0.5f;

        public static void Open()
        {
            Instance.StartCoroutine(Instance.Fade(1, true));
        }

        public static void Close()
        {
            Instance.StartCoroutine(Instance.Fade(0, false));
        }

        private IEnumerator Fade(float endValue, bool blocksRaycasts)
        {
            float startAlpha = m_canvasGroup.alpha;
            float rate = 1f / FadeTime;
            float progress = 0f;

            while (progress < 1f)
            {
                m_canvasGroup.alpha = Mathf.Lerp(startAlpha, endValue, progress);
                progress += rate * Time.deltaTime;

                yield return null;
            }

            m_canvasGroup.alpha = endValue;
            m_canvasGroup.blocksRaycasts = blocksRaycasts;
        }
    }
}