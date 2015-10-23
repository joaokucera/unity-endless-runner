using UnityEngine;

namespace EndlessRunner
{
    [AddComponentMenu("CUSTOM / Colors Manager")]
    public class ColorsManager : Singleton<ColorsManager>
    {
        [SerializeField]
        private Material[] m_materialColors;

        public static Material RandomColor()
        {
            int index = Random.Range(0, Instance.m_materialColors.Length);

            return Instance.m_materialColors[index];
        }
    }
}