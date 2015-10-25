using UnityEngine;

namespace EndlessRunner
{
    [RequireComponent(typeof(Renderer))]
    public abstract class ItemBase : MonoBehaviour
    {
        protected Renderer m_bodyRenderer;

        public Material CurrentMaterial { get { return m_bodyRenderer.sharedMaterial; } }
        public string CurrentMaterialName { get { return m_bodyRenderer.sharedMaterial.name; } }
        public abstract int HitPoints { get; }

        protected virtual void Start()
        {
            m_bodyRenderer = GetComponentInChildren<Renderer>();
            m_bodyRenderer.sharedMaterial = GameDirector.RandomColor();
        }

        void Update()
        {
            transform.Translate(-transform.forward * GlobalVariables.ScrollSpeed * Time.deltaTime);
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.IsExit())
            {
                Hide();
            }
        }

        protected void Hide()
        {
            gameObject.SetActive(false);

            m_bodyRenderer.sharedMaterial = GameDirector.RandomColor();
        }

        public abstract void Reset();

        protected abstract void OnCollisionEnter(Collision collision);
    }
}