using UnityEngine;

namespace EndlessRunner
{
    public abstract class ItemBase : MonoBehaviour
    {
        public Renderer m_bodyRenderer;
        private Collider m_collider;

        public Material CurrentMaterial { get { return m_bodyRenderer.sharedMaterial; } }
        public string CurrentMaterialName { get { return m_bodyRenderer.sharedMaterial.name; } }

        void Start()
        {
            m_collider = GetComponent<Collider>();
            m_bodyRenderer = GetComponentInChildren<Renderer>();

            m_bodyRenderer.sharedMaterial = ColorsManager.RandomColor();
        }

        void Update()
        {
            transform.Translate(-transform.forward * GlobalVariables.ScrollSpeed * Time.deltaTime);

            DoRaycast();
        }

        private void DoRaycast()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.forward, out hit, 1f))
            {
                if (hit.collider.IsExit())
                {
                    Reload();
                }
            }
        }

        protected void Show()
        {
            m_bodyRenderer.sharedMaterial = ColorsManager.RandomColor();
            m_bodyRenderer.enabled = true;

            m_collider.enabled = true;
        }

        public void Hide()
        {
            m_bodyRenderer.enabled = false;

            m_collider.enabled = false;
        }

        protected abstract void Reload();
    }
}