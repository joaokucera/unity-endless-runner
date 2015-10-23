using UnityEngine;

namespace EndlessRunner
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;

        protected static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<T>();
                }

                if (m_instance == null)
                {
                    string error = string.Format("An instance of {0} is needed in the scene, but there is none.", typeof(T));

                    Debug.LogError(error);
                }

                return m_instance;
            }
        }
    }
}