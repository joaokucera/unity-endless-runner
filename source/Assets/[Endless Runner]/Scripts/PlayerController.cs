using System;
using System.Collections;
using UnityEngine;

namespace EndlessRunner
{
    [AddComponentMenu("CUSTOM / Player Controller")]
    public class PlayerController : MonoBehaviour
    {
        private const int HitSpecialItemPoints = 25;
        private const int HitObstaclePoints = 10;

        [SerializeField]
        private Transform m_cameraTarget;
        [SerializeField]
        private float m_horizontalSpeed = 1, m_verticalSpeed = 1, m_fieldOfViewMultiplier = 1;
        [SerializeField]
        private ParticleSystem m_collisionEffectPrefab;
        [SerializeField]
        private Renderer[] m_bodyRenderers;

        private Rigidbody m_rigidbody;
        private ParticleSystem[] m_particles;

        private Vector3 m_movement;
        private int m_bestScore, m_currentScore;
        private bool m_canMove = true, m_canJump = true;
        private Vector3 m_playerStartPosition, m_targetStartLocalPosition;
        private Quaternion m_playerStartRotation, m_targetStartLocalRotation;

        private string MaterialName { get { return m_bodyRenderers[0].sharedMaterial.name; } }

        void Start()
        {
            Initialize();

            SetMaterialColors(ColorsManager.RandomColor());

            StartCoroutine("MoveCameraTargetIn");
        }

        void Update()
        {
            if (!m_canMove) return;

            // Jump.
            if (Input.GetButtonDown("Vertical") && m_canJump)
            {
                m_canJump = false;

                DoJump();
            }

            // Get movement value.
            m_movement = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

            // Check collision with raycast.
            DoRaycast();
        }

        void FixedUpdate()
        {
            if (!m_canMove) return;

            // Execute movement.
            m_rigidbody.MovePosition(m_rigidbody.position + m_movement * m_horizontalSpeed * Time.fixedDeltaTime);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.IsGround())
            {
                m_canJump = true;
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.IsSpecialItem())
            {
                var item = collider.GetComponent<ItemSpecial>();
                SetMaterialColors(item.CurrentMaterial);
                item.Hide();

                SoundManager.PlaySoundEffect("SpecialItemHit");

                AddScore(HitSpecialItemPoints);
            }
            else if (collider.IsWater())
            {
                Death();
            }
        }

        public void Reload()
        {
            StopCoroutine("MoveCameraTargetOut");

            // Restart player position and rotation.
            transform.position = m_playerStartPosition;
            transform.rotation = m_playerStartRotation;

            // Restart target position and rotation.
            m_cameraTarget.localPosition = m_targetStartLocalPosition;
            m_cameraTarget.localRotation = m_targetStartLocalRotation;

            m_currentScore = 0;
            SetMaterialColors(ColorsManager.RandomColor());

            HandleComponents(true);
            GlobalVariables.ScrollSpeed = GlobalVariables.OriginalSpeed;

            StartCoroutine("MoveCameraTargetIn");
        }

        private void Initialize()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            m_particles = GetComponentsInChildren<ParticleSystem>();

            // Save player start position and rotation.
            m_playerStartPosition = transform.position;
            m_playerStartRotation = transform.rotation;

            // Save target start position and rotation.
            m_targetStartLocalPosition = m_cameraTarget.localPosition;
            m_targetStartLocalRotation = m_cameraTarget.localRotation;
        }

        private void DoRaycast()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
            {
                if (hit.collider.IsObstacle())
                {
                    var obstacle = hit.collider.GetComponent<ItemObstacle>();

                    if (MaterialName == obstacle.CurrentMaterialName)
                    {
                        obstacle.Hide();

                        DoEffect("ObstacleHit");

                        AddScore(HitObstaclePoints);
                    }
                    else
                    {
                        Death();
                    }
                }
            }
        }

        private void Death()
        {
            m_rigidbody.velocity = Vector3.zero;
            StopCoroutine("Zoom");

            GlobalVariables.ScrollSpeed = 0;
            HandleComponents(false);

            DoEffect("PlayerDeath");

            StartCoroutine("MoveCameraTargetOut");

            Invoke("ShowScoreResults", 1f);
        }

        private void AddScore(int sum)
        {
            m_currentScore += sum;

            UIManager.UpdateCurrentScore(m_currentScore);
        }

        private void DoEffect(string clipName)
        {
            ScreenShake.Shake();

            SoundManager.PlaySoundEffect(clipName);

            var newEffect = Instantiate(m_collisionEffectPrefab, transform.position, Quaternion.identity) as ParticleSystem;
            Destroy(newEffect.gameObject, newEffect.startLifetime);
        }

        private void ShowScoreResults()
        {
            ScreenFade.Open();

            m_bestScore = Math.Max(m_bestScore, m_currentScore);

            UIManager.UpdateResultScores(m_currentScore, m_bestScore);
        }

        private void HandleComponents(bool enable)
        {
            SetRenderersVisibility(enable);

            m_rigidbody.isKinematic = !enable;
            m_canMove = enable;
            m_canJump = enable;
        }

        private void DoJump()
        {
            // Execute zoom (cam and world scroll).
            StartCoroutine("Zoom");

            for (int i = 0; i < m_particles.Length; i++)
            {
                m_particles[i].Play();
            }

            // Execute jump.
            m_rigidbody.AddRelativeForce(Vector3.up * m_verticalSpeed, ForceMode.Impulse);

            SoundManager.PlaySoundEffect("PlayerJump");
        }

        private IEnumerator Zoom()
        {
            var lastFieldOfView = GlobalVariables.CameraMain.fieldOfView;
            var lastScrollSpeed = GlobalVariables.ScrollSpeed;

            while (!m_canJump)
            {
                GlobalVariables.CameraMain.fieldOfView += Time.deltaTime * m_fieldOfViewMultiplier;
                GlobalVariables.CameraMain.fieldOfView = Math.Min(GlobalVariables.CameraMain.fieldOfView, 80f);

                GlobalVariables.ScrollSpeed += Time.deltaTime;
                GlobalVariables.ScrollSpeed = Math.Min(GlobalVariables.ScrollSpeed, lastScrollSpeed * 2f);

                yield return null;
            }

            while (GlobalVariables.CameraMain.fieldOfView > lastFieldOfView)
            {
                GlobalVariables.CameraMain.fieldOfView -= Time.deltaTime * m_fieldOfViewMultiplier;
                GlobalVariables.CameraMain.fieldOfView = Math.Max(GlobalVariables.CameraMain.fieldOfView, lastFieldOfView);

                GlobalVariables.ScrollSpeed -= Time.deltaTime;
                GlobalVariables.ScrollSpeed = Math.Max(GlobalVariables.ScrollSpeed, lastScrollSpeed);

                yield return null;
            }

            GlobalVariables.CameraMain.fieldOfView = lastFieldOfView;
            GlobalVariables.ScrollSpeed = lastScrollSpeed;
        }

        private IEnumerator MoveCameraTargetIn()
        {
            yield return new WaitForSeconds(1f);

            while (m_cameraTarget.localPosition.y > 0.1f)
            {
                m_cameraTarget.localPosition = Vector3.Lerp(m_cameraTarget.localPosition, Vector3.zero, Time.deltaTime);
                m_cameraTarget.localRotation = Quaternion.Lerp(m_cameraTarget.localRotation, Quaternion.identity, Time.deltaTime);

                yield return null;
            }

            m_cameraTarget.localPosition = Vector3.zero;
            m_cameraTarget.localRotation = Quaternion.identity;
        }

        private IEnumerator MoveCameraTargetOut()
        {
            while (m_cameraTarget.localPosition.y < 0.9f)
            {
                m_cameraTarget.localPosition = Vector3.Lerp(m_cameraTarget.localPosition, m_targetStartLocalPosition, Time.deltaTime);
                m_cameraTarget.localRotation = Quaternion.Lerp(m_cameraTarget.localRotation, m_targetStartLocalRotation, Time.deltaTime);

                yield return null;
            }

            m_cameraTarget.localPosition = m_targetStartLocalPosition;
            m_cameraTarget.localRotation = m_targetStartLocalRotation;
        }

        private void SetMaterialColors(Material materialColor)
        {
            for (int i = 0; i < m_bodyRenderers.Length; i++)
            {
                m_bodyRenderers[i].sharedMaterial = materialColor;
            }
        }

        private void SetRenderersVisibility(bool enable)
        {
            for (int i = 0; i < m_bodyRenderers.Length; i++)
            {
                m_bodyRenderers[i].enabled = enable;
            }
        }
    }
}