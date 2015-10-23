using UnityEngine;
using System.Linq;

namespace EndlessRunner
{
    public class GenericSpawner : MonoBehaviour
    {
        [SerializeField]
        private ItemBase m_prefab;
        [SerializeField]
        private int m_length;
        [SerializeField]
        private float m_waitTimeToGenerate;
        [SerializeField]
        private float m_distanceBetweenObjects;

        private ItemBase[] m_objects;

        void Start()
        {
            m_objects = new ItemBase[m_length];

            Invoke("Generate", m_waitTimeToGenerate);
        }

        private void Generate()
        {
            Vector3 startPosition = transform.localPosition;

            for (int i = 0; i < m_objects.Length; i++)
            {
                startPosition.z += m_distanceBetweenObjects;

                var newClone = Instantiate(m_prefab, startPosition, Quaternion.identity) as ItemBase;
                newClone.transform.SetParent(transform);

                m_objects[i] = newClone;
            }
        }

        public void Reload()
        {
            for (int i = 0; i < m_objects.Length; i++)
            {
                Destroy(m_objects[i].gameObject);
            }

            Invoke("Generate", m_waitTimeToGenerate);
        }
    }
}