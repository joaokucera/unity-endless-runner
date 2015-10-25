using UnityEngine;

namespace EndlessRunner
{
    public class Spawner : GenericPooling
    {
        [SerializeField]
        private float m_minSpawnTime = 1, m_maxSpawnTime = 1;
        [SerializeField]
        private bool m_showDebug;

        private float m_originalMinSpawnTime, m_originalMaxSpawnTime, m_limitMinSpawnTime, m_limitMaxSpawnTime;

        void Start()
        {
            m_originalMinSpawnTime = m_minSpawnTime;
            m_originalMaxSpawnTime = m_maxSpawnTime;

            m_limitMinSpawnTime = m_originalMinSpawnTime / 2f;
            m_limitMaxSpawnTime = m_originalMaxSpawnTime / 2f;

            InvokeSpawn();
        }

        void OnGUI()
        {
            if (m_showDebug)
                GUI.Label(new Rect(Screen.width - 200, 40, 200, 100), "Min: " + m_minSpawnTime + " | Max: " + m_maxSpawnTime);
        }

        public void Reload()
        {
            DeactivateAll();

            m_minSpawnTime = m_originalMinSpawnTime;
            m_maxSpawnTime = m_originalMaxSpawnTime;

            InvokeSpawn();
        }

        public void Stop()
        {
            CancelInvoke("Spawn");
        }

        private void Spawn()
        {
            var newItem = GetObjectFromPool();

            if (newItem != null)
                newItem.Reset();

            InvokeSpawn();
        }

        private void InvokeSpawn()
        {
            var time = Random.Range(m_minSpawnTime, m_maxSpawnTime);

            var minus = Time.deltaTime / 5f;
            if (m_minSpawnTime > m_limitMinSpawnTime) m_minSpawnTime -= minus;
            if (m_maxSpawnTime > m_limitMaxSpawnTime) m_maxSpawnTime -= minus;

            Invoke("Spawn", time);
        }
    }
}