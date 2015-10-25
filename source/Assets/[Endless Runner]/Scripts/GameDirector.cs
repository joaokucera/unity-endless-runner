using UnityEngine;

namespace EndlessRunner
{
    [AddComponentMenu("CUSTOM / Game Director")]
    public class GameDirector : Singleton<GameDirector>
    {
        [SerializeField]
        private ParticleSystem m_collisionEffectPrefab;
        [SerializeField]
        private Material[] m_materialColors;
        [SerializeField]
        private bool m_showDebug;

        private Spawner[] m_genericSpawner;
        private int m_bestScore, m_currentScore;
        private float m_originalScrollSpeed, m_limitScrollSpeed;
        private int m_lastIndexRandomized;
        private bool m_isPlaying;

        void Start()
        {
            m_genericSpawner = FindObjectsOfType<Spawner>();

            m_originalScrollSpeed = GlobalVariables.ScrollSpeed;
            m_limitScrollSpeed = m_originalScrollSpeed * 2f;

            m_isPlaying = true;
        }

        void Update()
        {
            if (m_isPlaying && GlobalVariables.ScrollSpeed < m_limitScrollSpeed)
            {
                GlobalVariables.ScrollSpeed += Time.deltaTime / 5f;
            }
        }

        void OnGUI()
        {
            if (m_showDebug)
                GUI.Label(new Rect(Screen.width - 200, 20, 200, 100), "ScrollSpeed: " + GlobalVariables.ScrollSpeed);
        }

        public static Material RandomColor()
        {
            var length = Instance.m_materialColors.Length;
            var index = Random.Range(0, length);

            if (index == Instance.m_lastIndexRandomized)
            {
                if (index == length - 1) index = 0;
                else index++;
            }

            Instance.m_lastIndexRandomized = index;

            return Instance.m_materialColors[index];
        }

        public static void DoEffects(string soundEffectClipName, Material material)
        {
            ScreenShake.Shake();

            SoundManager.PlaySoundEffect(soundEffectClipName);

            var newEffect = Instantiate(Instance.m_collisionEffectPrefab, GlobalVariables.Player.transform.position, Quaternion.identity) as ParticleSystem;
            newEffect.GetComponent<Renderer>().sharedMaterial = material;

            Destroy(newEffect.gameObject, newEffect.startLifetime);
        }

        public static void AddScore(int sum)
        {
            Instance.m_currentScore += sum;

            UIManager.UpdateCurrentScore(Instance.m_currentScore);
        }

        public static void ShowScoreResults()
        {
            for (int i = 0; i < Instance.m_genericSpawner.Length; i++)
            {
                Instance.m_genericSpawner[i].Stop();
            }

            ScreenFade.Open();

            Instance.m_bestScore = Mathf.Max(Instance.m_bestScore, Instance.m_currentScore);

            UIManager.UpdateResultScores(Instance.m_currentScore, Instance.m_bestScore);
        }

        public static void StopGame()
        {
            Instance.m_isPlaying = false;

            GlobalVariables.ScrollSpeed = 0;
        }

        public static void ResetGame()
        {
            for (int i = 0; i < Instance.m_genericSpawner.Length; i++)
            {
                Instance.m_genericSpawner[i].Reload();
            }

            GlobalVariables.ScrollSpeed = Instance.m_originalScrollSpeed;

            Instance.m_currentScore = 0;

            Instance.m_isPlaying = true;
        }
    }
}