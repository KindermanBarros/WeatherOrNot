using UnityEngine;
using WeatherOrNot.Events.Weather;
using WeatherOrNot.Utils;

namespace WeatherOrNot.App.PlayerManager
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")] [SerializeField]
        private float m_moveSpeed = 8f;

        [SerializeField] private float m_jumpUpwardSpeed = 8f;
        [SerializeField] private float m_wallJumpForce = 10f;
        [SerializeField] private Vector2 m_wallJumpDirection = new(1, 1);

        [Header("Jump Gravity Settings")] [SerializeField]
        private float m_fallMultiplier = 2.5f;

        [SerializeField] private float m_lowJumpMultiplier = 2f;

        [Header("Tolerance Time")] [SerializeField]
        private float m_coyoteTime = 0.2f;

        [SerializeField] private float m_jumpBufferTime = 0.05f;

        [Header("Wall Slide")] [SerializeField]
        private float m_wallSlideSpeed = 2f;

        [Header("Dash Settings")] [SerializeField]
        private float m_dashSpeed = 20f;

        [SerializeField] private float m_dashDuration = 0.2f;
        [SerializeField] private float m_dashCooldown = 1f;

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

        private void Awake()
        {
            m_rb = GetComponent<Rigidbody2D>();
            m_lastGroundedTime = Time.time;
        }

        private void Update()
        {
            if (Time.time < 0.1f) return;

            HandleInput();
            m_horizontalInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump"))
                m_lastJumpPressedTime = Time.time;

            if (Input.GetKeyDown(KeyCode.Z))
                EventBus.Notify(this, new ChangeWeatherEvent(WeatherTypes.Snow));
            if (Input.GetKeyDown(KeyCode.X))
                EventBus.Notify(this, new ChangeWeatherEvent(WeatherTypes.Clear));
            if (Input.GetKeyDown(KeyCode.C))
                EventBus.Notify(this, new ChangeWeatherEvent(WeatherTypes.Thunderstorm));
            if (Input.GetKeyDown(KeyCode.V))
                EventBus.Notify(this, new ChangeWeatherEvent(WeatherTypes.Windy));
            if (Input.GetKeyDown(KeyCode.B))
                EventBus.Notify(this, new ChangeWeatherEvent(WeatherTypes.Rain));

            FlipCharacter(m_horizontalInput);
            HandleWallSlide();
            HandleJump();
            HandleAnimations();
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

        private void HandleInput()
        {
            m_horizontalInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump"))
                m_lastJumpPressedTime = Time.time;

            if (Input.GetKeyDown(KeyCode.Q))
                TryDash();

            if (Input.GetKeyDown(KeyCode.Z)) SetSnowy();
            if (Input.GetKeyDown(KeyCode.X)) SetSunny();
            if (Input.GetKeyDown(KeyCode.C)) SetThunderstorm();
            if (Input.GetKeyDown(KeyCode.V)) SetWindy();
            if (Input.GetKeyDown(KeyCode.B)) SetRain();
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
            //TODO: Play Dash Animation

            if (Time.time > m_dashTime)
            {
                m_isDashing = false;
                //TODO: End Dash Animation
            }
        }

        private void HandleWallSlide()
        {
            m_isWallSliding = m_isTouchingWall && !m_isGrounded && m_rb.linearVelocity.y < 0f;
            if (m_isWallSliding)
            {
                //TODO: Play Wall Slide Animation
            }
        }

        private void HandleJump()
        {
            var jumpBuffered = Time.time - m_lastJumpPressedTime <= m_jumpBufferTime;
            var canUseCoyote = Time.time - m_lastGroundedTime <= m_coyoteTime;

            if (!jumpBuffered) return;

            if (m_isWallSliding)
            {
                var force = new Vector2(-m_wallDirection * m_wallJumpDirection.x * m_wallJumpForce,
                    m_wallJumpDirection.y * m_wallJumpForce);
                m_rb.linearVelocity = Vector2.zero;
                m_rb.AddForce(force, ForceMode2D.Impulse);
                m_lastJumpPressedTime = -1;

                //TODO: Play Wall Jump Animation
            }
            else if (m_isGrounded || canUseCoyote)
            {
                m_rb.linearVelocity = new Vector2(m_rb.linearVelocity.x, m_jumpUpwardSpeed);
                m_lastJumpPressedTime = -1;

                //TODO: Play Jump Animation
            }
        }

        private void HandleAnimations()
        {
            if (m_isDashing) return;

            if (!m_isGrounded)
            {
                if (m_rb.linearVelocity.y > 0)
                {
                    //TODO: Play Jump Rising Animation
                }
                else if (m_rb.linearVelocity.y < 0)
                {
                    //TODO: Play Falling Animation
                }

                return;
            }

            if (Mathf.Abs(m_horizontalInput) > 0.1f)
            {
                //TODO: Play Walking Animation
            }
            else
            {
                //TODO: Play Idle Animation
            }
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

            m_isDashing = true;
            m_dashTime = Time.time + m_dashDuration;
            m_lastDashTime = Time.time;
        }

        private void TryChangeWeather(WeatherTypes weather)
        {
            EventBus.Notify(this, new ChangeWeatherEvent(weather));
        }

        private void SetRain() => TryChangeWeather(WeatherTypes.Rain);
        private void SetSnowy() => TryChangeWeather(WeatherTypes.Snow);
        private void SetSunny() => TryChangeWeather(WeatherTypes.Clear);
        private void SetThunderstorm() => TryChangeWeather(WeatherTypes.Thunderstorm);
        private void SetWindy() => TryChangeWeather(WeatherTypes.Windy);

        private void OnCollisionStay2D(Collision2D collision)
        {
            m_isGrounded = false;
            m_isTouchingWall = false;

            foreach (var contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    m_isGrounded = true;
                    m_lastGroundedTime = Time.time;
                }
                else if (Mathf.Abs(contact.normal.x) > 0.5f)
                {
                    m_isTouchingWall = true;
                    m_wallDirection = contact.normal.x > 0 ? -1 : 1;
                }
            }
        }

        private void OnCollisionExit2D()
        {
            m_isGrounded = false;
            m_isTouchingWall = false;
        }
    }
}