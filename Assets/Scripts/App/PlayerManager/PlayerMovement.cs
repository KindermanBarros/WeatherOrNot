using UnityEngine;

namespace WeatherOrNot.App.PlayerManager
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")] [SerializeField]
        private float m_moveSpeed = 8f;

        [SerializeField] private float m_jumpForce = 8f;
        [SerializeField] private float m_wallJumpForce = 10f;
        [SerializeField] private Vector2 m_wallJumpDirection = new(1, 1);


        [Header("Tolerance Time")] [SerializeField]
        private float m_coyoteTime = 0.2f;

        [SerializeField] private float m_jumpBufferTime = 0.15f;


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
            m_rb.linearVelocity = Vector2.zero;
        }

        private void Update()
        {
            if (Time.time < 0.1f) return;

            m_horizontalInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump"))
                m_lastJumpPressedTime = Time.time;

            if (Input.GetKeyDown(KeyCode.Q))
                TryDash();

            FlipCharacter(m_horizontalInput);
            HandleWallSlide();
            HandleJump();
        }

        private void FixedUpdate()
        {
            if (m_isDashing)
            {
                m_rb.linearVelocity = new Vector2((m_isFacingRight ? 1 : -1) * m_dashSpeed, 0);
                if (Time.time > m_dashTime)
                {
                    m_isDashing = false;
                }

                return;
            }

            var _currentVelocity = m_rb.linearVelocity;
            _currentVelocity.x = m_horizontalInput * m_moveSpeed;
            m_rb.linearVelocity = new Vector2(_currentVelocity.x, m_rb.linearVelocity.y);

            if (m_isWallSliding)
            {
                m_rb.linearVelocity = new Vector2(m_rb.linearVelocity.x, -m_wallSlideSpeed);
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
                    var _scale = transform.localScale;
                    _scale.x *= -1;
                    transform.localScale = _scale;
                    break;
                }
            }
        }

        private void HandleWallSlide()
        {
            m_isWallSliding = m_isTouchingWall && !m_isGrounded && m_rb.linearVelocity.y < 0f;
        }

        private void HandleJump()
        {
            var _jumpBuffered = Time.time - m_lastJumpPressedTime <= m_jumpBufferTime;
            var _canUseCoyote = Time.time - m_lastGroundedTime <= m_coyoteTime;

            if (!_jumpBuffered) return;

            if (m_isWallSliding)
            {
                var _force = new Vector2(-m_wallDirection * m_wallJumpDirection.x * m_wallJumpForce,
                    m_wallJumpDirection.y * m_wallJumpForce);
                m_rb.linearVelocity = Vector2.zero;
                m_rb.AddForce(_force, ForceMode2D.Impulse);
                m_lastJumpPressedTime = -1;
            }
            else if (m_isGrounded || _canUseCoyote)
            {
                m_rb.linearVelocity = new Vector2(m_rb.linearVelocity.x, m_jumpForce);
                m_lastJumpPressedTime = -1;
            }
        }

        private void TryDash()
        {
            if (Time.time < m_lastDashTime + m_dashCooldown)
                return;

            m_isDashing = true;
            m_dashTime = Time.time + m_dashDuration;
            m_lastDashTime = Time.time;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            m_isGrounded = false;
            m_isTouchingWall = false;

            foreach (var _contact in collision.contacts)
            {
                if (_contact.normal.y > 0.5f)
                {
                    m_isGrounded = true;
                    m_lastGroundedTime = Time.time;
                }
                else if (Mathf.Abs(_contact.normal.x) > 0.5f)
                {
                    m_isTouchingWall = true;
                    m_wallDirection = _contact.normal.x > 0 ? -1 : 1;
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