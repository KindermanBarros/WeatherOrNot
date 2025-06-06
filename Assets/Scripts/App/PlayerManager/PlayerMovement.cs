using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WeatherOrNot.Events.Animation;
using WeatherOrNot.Events.Weather;
using WeatherOrNot.Utils;
using FMOD.Studio;
using WeatherOrNot.App;

namespace WeatherOrNot.App.PlayerManager
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField]
        private float m_moveSpeed = 8f;
        [SerializeField] private float m_jumpUpwardSpeed = 8f;
        [SerializeField] private float m_wallJumpForce = 10f;
        [SerializeField] private Vector2 m_wallJumpDirection = new(1, 1);


        [Header("Jump Gravity Settings")]
        [SerializeField]
        private float m_fallMultiplier = 2.5f;
        [SerializeField] private float m_lowJumpMultiplier = 2f;


        [Header("Tolerance Time")]
        [SerializeField]
        private float m_coyoteTime = 0.2f;
        [SerializeField] private float m_jumpBufferTime = 0.05f;


        [Header("Wall Slide")]
        [SerializeField]
        private float m_wallSlideSpeed = 2f;


        [Header("Dash Settings")]
        [SerializeField]
        private float m_dashSpeed = 20f;
        [SerializeField] private float m_dashDuration = 0.2f;
        [SerializeField] private float m_dashCooldown = 1f;


        [Header("Audio Settings")]
        private EventInstance m_footstepSound;


        private Rigidbody2D m_rb;
        private float m_horizontalInput;

        private bool m_isGrounded;
        private bool m_isTouchingWall;
        private bool m_isWallSliding;
        private int m_wallDirection;

        private float m_lastGroundedTime;
        private float m_lastJumpPressedTime = -Mathf.Infinity;
        private bool m_isFacingRight = true;

        private bool m_isDashing;
        private float m_dashTime;
        private float m_lastDashTime;

        private float m_wallJumpLockTime = 0.10f;
        private float m_lastWallJumpTime = -Mathf.Infinity;
        private float m_groundLockTime = 0.08f;
        private float m_lastLeftGroundTime = -Mathf.Infinity;

        private void Awake()
        {
            m_rb = GetComponent<Rigidbody2D>();
            m_lastGroundedTime = Time.time;
            m_rb.linearVelocity = Vector2.zero;
        }

        private void Start()
        {
            m_footstepSound = AudioManager.instance.CreateInstance(FMODEvents.instance.playerFootsteps);
        }

        private void Update()
        {
            if (Time.time < 0.1f) return;

            m_horizontalInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump"))
                m_lastJumpPressedTime = Time.time;

            if (Input.GetKeyDown(KeyCode.Z))
                EventBus.Notify(this, new ChangeWeatherEvent(WeatherTypes.Clear));
            if (Input.GetKeyDown(KeyCode.X))
                EventBus.Notify(this, new ChangeWeatherEvent(WeatherTypes.Rain));
            if (Input.GetKeyDown(KeyCode.C))
                EventBus.Notify(this, new ChangeWeatherEvent(WeatherTypes.Snow));
            if (Input.GetKeyDown(KeyCode.V))
                EventBus.Notify(this, new ChangeWeatherEvent(WeatherTypes.Thunderstorm));
            if (Input.GetKeyDown(KeyCode.B))
                EventBus.Notify(this, new ChangeWeatherEvent(WeatherTypes.Windy));

            FlipCharacter(m_horizontalInput);
            HandleWallSlide();
            HandleJump();
            HandleAnimations();
            UpdateSound();
        }

        private void FixedUpdate()
        {
            if (m_isDashing)
            {
                ApplyDash();
                return;
            }

            ApplyMovement();

            if (!m_isWallSliding)
            {
                var velocity = m_rb.linearVelocity;

                if (velocity.y < 0)
                {
                    velocity.y += Physics2D.gravity.y * (m_fallMultiplier - 1) * Time.fixedDeltaTime;
                    m_rb.linearVelocity = velocity;
                }
                else if (velocity.y > 0 && !Input.GetButton("Jump"))
                {
                    velocity.y += Physics2D.gravity.y * (m_lowJumpMultiplier - 1) * Time.fixedDeltaTime;
                    m_rb.linearVelocity = velocity;
                }
            }

            ApplyWallSlide();
        }

        private void ApplyMovement()
        {
            var currentVelocity = m_rb.linearVelocity;
            currentVelocity.x = m_horizontalInput * m_moveSpeed;
            m_rb.linearVelocity = new Vector2(currentVelocity.x, m_rb.linearVelocity.y);
        }

        private void ApplyWallSlide()
        {
            if (m_isWallSliding)
            {
                m_rb.linearVelocity = new Vector2(m_rb.linearVelocity.x, -m_wallSlideSpeed);
            }
        }

        private void ApplyDash()
        {
            m_rb.linearVelocity = new Vector2((m_isFacingRight ? 1 : -1) * m_dashSpeed, 0);
            EventBus.Notify(this, new StartDashEvent());

            if (Time.time > m_dashTime)
            {
                m_isDashing = false;
            }
        }

        private void HandleWallSlide()
        {
            bool wasWallSliding = m_isWallSliding;
            m_isWallSliding = m_isTouchingWall && !m_isGrounded && m_rb.linearVelocity.y < 0f;

            if (m_isWallSliding && !wasWallSliding)
            {
                EventBus.Notify(this, new StartWallSlidingEvent());

                m_lastDashTime = -Mathf.Infinity;
            }
        }

        private void HandleJump()
        {
            var jumpBuffered = Time.time - m_lastJumpPressedTime <= m_jumpBufferTime;
            if (!jumpBuffered) return;

            var wallJumpLock = Time.time - m_lastWallJumpTime <= m_wallJumpLockTime;

            if (m_isWallSliding && !wallJumpLock)
            {
                var force = new Vector2(-m_wallDirection * m_wallJumpDirection.x * m_wallJumpForce,
                    m_wallJumpDirection.y * m_wallJumpForce);
                m_rb.linearVelocity = Vector2.zero;
                m_rb.AddForce(force, ForceMode2D.Impulse);
                m_lastJumpPressedTime = -1;

                EventBus.Notify(this, new StartWallJumpingEvent());

                //TODO: Play Wall Jump Animation
                ExecuteWallJump();
                return;
            }

            var canUseCoyote = Time.time - m_lastGroundedTime <= m_coyoteTime;
            if ((m_isGrounded || canUseCoyote) && !wallJumpLock)
            {
                m_rb.linearVelocity = new Vector2(m_rb.linearVelocity.x, m_jumpUpwardSpeed);
                m_lastJumpPressedTime = -1;
                EventBus.Notify(this, new StartJumpingEvent());

                //TODO: Play Jump Animation
                ExecuteNormalJump();
            }
        }

        private void ExecuteWallJump()
        {
            var force = new Vector2(-m_wallDirection * m_wallJumpDirection.x * m_wallJumpForce,
                m_wallJumpDirection.y * m_wallJumpForce);

            m_rb.linearVelocity = Vector2.zero;
            m_rb.AddForce(force, ForceMode2D.Impulse);

            m_lastJumpPressedTime = -1;
            m_lastWallJumpTime = Time.time;
            m_lastLeftGroundTime = Time.time;

            m_isWallSliding = false;
            m_isTouchingWall = false;
            m_isGrounded = false;

            AudioManager.instance.PlayOneShot(FMODEvents.instance.playerJump, transform.position);

            var originalFacing = m_isFacingRight;
            if ((m_wallDirection == -1 && !m_isFacingRight) || (m_wallDirection == 1 && m_isFacingRight))
            {
                FlipCharacter(m_isFacingRight ? -1 : 1);
            }

            RestoreOriginalFacing(originalFacing).Forget();

            EventBus.Notify(this, new StartWallJumpingEvent());
        }

        private async UniTask RestoreOriginalFacing(bool originalFacing)
        {
            await UniTask.Delay(1000);

            if (!m_isGrounded)
            {
                if (originalFacing != m_isFacingRight)
                {
                    var scale = transform.localScale;
                    scale.x *= -1;
                    transform.localScale = scale;
                    m_isFacingRight = originalFacing;
                }
            }
        }

        private void ExecuteNormalJump()
        {
            m_rb.linearVelocity = new Vector2(m_rb.linearVelocity.x, m_jumpUpwardSpeed);
            m_lastJumpPressedTime = -1;

            AudioManager.instance.PlayOneShot(FMODEvents.instance.playerJump, transform.position);

            EventBus.Notify(this, new StartJumpingEvent());
        }

        private void HandleAnimations()
        {
            if (m_isDashing) return;

            if (!m_isGrounded)
            {
                if (m_rb.linearVelocity.y > 0)
                {
                    EventBus.Notify(this, new StartJumpingEvent());
                }
                else if (m_rb.linearVelocity.y < 0)
                {
                    EventBus.Notify(this, new StartEndJumpingEvent());
                }

                return;
            }

            if (Mathf.Abs(m_horizontalInput) > 0.1f)
            {
                EventBus.Notify(this, new StartWalkingEvent());
            }
            else
            {
                EventBus.Notify(this, new StartIdleEvent());
            }
        }

        private void SetRain()
        {
            TryChangeWeather(WeatherTypes.Rain);
        }

        private void SetSnowy()
        {
            TryChangeWeather(WeatherTypes.Snow);
        }

        private void SetSunny()
        {
            TryChangeWeather(WeatherTypes.Clear);
        }

        private void SetThunderstorm()
        {
            TryChangeWeather(WeatherTypes.Thunderstorm);
        }

        private void SetWindy()
        {
            TryChangeWeather(WeatherTypes.Windy);
        }

        private void FlipCharacter(float direction)
        {
            switch (direction)
            {
                case 0:
                    return;
                case > 0 when !m_isFacingRight:
                case < 0 when m_isFacingRight:
                    {
                        m_isFacingRight = !m_isFacingRight;
                        var scale = transform.localScale;
                        scale.x *= -1;
                        transform.localScale = scale;
                        break;
                    }
            }
        }

        private void TryDash()
        {
            if (Time.time < m_lastDashTime + m_dashCooldown) return;

            if (!m_isGrounded && m_lastDashTime > m_lastGroundedTime) return;

            m_isDashing = true;
            m_dashTime = Time.time + m_dashDuration;
            m_lastDashTime = Time.time;
        }

        private void TryChangeWeather(WeatherTypes weather)
        {
            EventBus.Notify(this, new ChangeWeatherEvent(weather));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ProcessCollisionContacts(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            ProcessCollisionContacts(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            var stillGrounded = false;
            var stillTouchingWall = false;

            if (collision.contactCount > 0)
            {
                foreach (var contact in collision.contacts)
                {
                    if (contact.normal.y > 0.5f)
                    {
                        stillGrounded = true;
                    }
                    else if (Mathf.Abs(contact.normal.x) > 0.5f)
                    {
                        stillTouchingWall = true;
                        m_wallDirection = contact.normal.x > 0 ? -1 : 1;
                    }
                }
            }

            if (!stillGrounded || !stillTouchingWall)
            {
                var contacts = new Collider2D[10];
                var filter = new ContactFilter2D().NoFilter();
                filter.useTriggers = false;

                int numContacts = GetComponent<Collider2D>().Overlap(filter, contacts);

                for (int i = 0; i < numContacts; i++)
                {
                    if (contacts[i] == collision.collider) continue;

                    Vector2 direction = (contacts[i].transform.position - transform.position).normalized;

                    if (!stillGrounded && Vector2.Dot(Vector2.up, direction) < -0.5f)
                    {
                        stillGrounded = true;
                    }
                    else if (!stillTouchingWall && Mathf.Abs(Vector2.Dot(Vector2.right, direction)) > 0.5f)
                    {
                        stillTouchingWall = true;
                        m_wallDirection = Vector2.Dot(Vector2.right, direction) > 0 ? -1 : 1;
                    }

                    if (stillGrounded && stillTouchingWall) break;
                }
            }

            if (!stillGrounded)
            {
                m_isGrounded = false;
                m_lastLeftGroundTime = Time.time;
            }

            if (!stillTouchingWall)
            {
                m_isTouchingWall = false;
            }
        }

        private void ProcessCollisionContacts(Collision2D collision)
        {
            if (collision.contactCount <= 0) return;

            bool foundGround = false;
            bool foundWall = false;

            foreach (var contact in collision.contacts)
            {
                if (!foundGround && contact.normal.y > 0.5f)
                {
                    if (Time.time - m_lastWallJumpTime > m_groundLockTime)
                    {
                        if (!m_isGrounded)
                        {
                            AudioManager.instance.PlayOneShot(FMODEvents.instance.playerLand, transform.position);
                        }

                        foundGround = true;
                        m_isGrounded = true;
                        m_lastGroundedTime = Time.time;
                    }
                }
                else if (!foundWall && Mathf.Abs(contact.normal.x) > 0.5f)
                {
                    foundWall = true;
                    m_isTouchingWall = true;
                    m_wallDirection = contact.normal.x > 0 ? -1 : 1;
                }

                if (foundGround && foundWall) break;
            }
        }

        private void UpdateSound()
        {
            if (Mathf.Abs(m_rb.linearVelocity.x) > 0.1f && m_isGrounded)
            {
                PLAYBACK_STATE playbackState;
                m_footstepSound.getPlaybackState(out playbackState);

                if (playbackState != PLAYBACK_STATE.PLAYING)
                {
                    m_footstepSound.start();
                }
            }
            else
            {
                m_footstepSound.stop(STOP_MODE.ALLOWFADEOUT);
            }
        }
    }
}
