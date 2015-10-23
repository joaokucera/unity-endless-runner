using UnityEngine;

namespace EndlessRunner
{
    public class GlobalVariables : ScriptableObject
    {
        public static float ScrollSpeed = 35f;
        public static float OriginalSpeed = ScrollSpeed;

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
    }
}