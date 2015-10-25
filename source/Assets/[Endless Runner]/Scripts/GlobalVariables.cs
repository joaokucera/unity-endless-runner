using UnityEngine;

namespace EndlessRunner
{
    public class GlobalVariables : ScriptableObject
    {
        public static float ScrollSpeed = 25f;

        public static bool SpawnAtPlayerDirection = true;

        private static Camera m_cameraMain;
        public static Camera CameraMain
        {
            get
            {
                if (m_cameraMain == null)
                    m_cameraMain = Camera.main;

                return m_cameraMain;
            }
        }

        private static PlayerController m_player;
        public static PlayerController Player
        {
            get
            {
                if (m_player == null)
                    m_player = FindObjectOfType<PlayerController>();

                return m_player;
            }
        }
    }
}