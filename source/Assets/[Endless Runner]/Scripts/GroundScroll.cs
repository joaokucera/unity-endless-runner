using UnityEngine;

namespace EndlessRunner
{
    [AddComponentMenu("CUSTOM / Ground Scroll")]
    [RequireComponent(typeof(Renderer))]
    public class GroundScroll : Singleton<GroundScroll>
    {
        [SerializeField]
        private float m_scrollDivider;

        private Renderer m_renderer;
        private float m_offset = 0f;

        void Start()
        {
            m_renderer = GetComponent<Renderer>();
        }

        void Update()
        {
            m_offset -= Time.deltaTime * (GlobalVariables.ScrollSpeed / m_scrollDivider);

            m_renderer.material.mainTextureOffset = new Vector3(0, m_offset, 0);
        }
    }
}