using UnityEngine;
using WeatherOrNot.Events.Animation;
using WeatherOrNot.Utils;

namespace WeatherOrNot.App.PlayerManager
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovement m_playerMovement;

        [Header("Configuration")] [SerializeField]
        private string m_dangerTag;

        [SerializeField] private float m_respawnDelay = 1.0f;

        private Vector3 m_initialPosition;
        private bool m_isDead = false;

        private void Awake()
        {
            if (m_playerMovement == null)
                m_playerMovement = GetComponent<PlayerMovement>();

            m_initialPosition = transform.position;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(m_dangerTag))
            {
                KillPlayer();
            }
        }

        private void OnTriggerEnter2D(Collider2D colliderTrigger)
        {
            if (colliderTrigger.CompareTag(m_dangerTag))
            {
                KillPlayer();
            }
        }

        public void KillPlayer()
        {
            if (m_isDead)
                return;

            m_isDead = true;

            AudioManager.instance.PlayOneShot(FMODEvents.instance.playerDeath, transform.position);

            m_playerMovement.enabled = false;

            if (TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Dynamic;
            }

            // TODO: Add Death Animation and Effects
            // EventBus.Notify(this, new StartDeadEvent());

            Invoke(nameof(RespawnPlayer), m_respawnDelay);
        }

        private void RespawnPlayer()
        {
            transform.position = m_initialPosition;

            if (m_playerMovement != null)
                m_playerMovement.enabled = true;

            if (TryGetComponent<Rigidbody2D>(out var _rb))
                _rb.bodyType = RigidbodyType2D.Dynamic;

            m_isDead = false;
        }

        public void SetRespawnPoint(Vector3 position)
        {
            m_initialPosition = position;
        }
    }
}