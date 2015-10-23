using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessRunner
{
    [AddComponentMenu("CUSTOM / UI Manager")]
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField]
        private Text m_textCurrentScore;
        [SerializeField]
        private Text m_textFinalScore;
        [SerializeField]
        private Text m_textBestScore;
        [SerializeField]
        private Button m_buttonPlay;

        private PlayerController m_playerController;
        private GenericSpawner[] m_genericSpawner;

        void Start()
        {
            m_playerController = FindObjectOfType<PlayerController>();
            m_genericSpawner = FindObjectsOfType<GenericSpawner>();

            m_buttonPlay.onClick.AddListener(RestartGame);
        }

#if !UNITY_WEBGL && !UNITY_WEBPLAYER
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }
#endif

        public static void UpdateCurrentScore(int currentScore)
        {
            Instance.m_textCurrentScore.text = string.Format("SCORE: {0}", currentScore);
        }

        public static void UpdateResultScores(int finalScore, int bestScore)
        {
            Instance.m_textFinalScore.text = string.Format("FINAL SCORE: {0}", finalScore);

            Instance.m_textBestScore.text = string.Format("BEST SCORE: {0}", bestScore);
        }

        private void RestartGame()
        {
            SoundManager.PlaySoundEffect("ClickButton");

            ScreenFade.Close();

            for (int i = 0; i < m_genericSpawner.Length; i++)
            {
                m_genericSpawner[i].Reload();
            }

            m_playerController.Reload();
        }
    }
}