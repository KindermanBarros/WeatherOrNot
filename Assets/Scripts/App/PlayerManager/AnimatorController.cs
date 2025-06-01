using UnityEngine;
using WeatherOrNot.Events.Animation;
using WeatherOrNot.Utils;

namespace  WeatherOrNot.App.PlayerManager
{
    public class AnimatorController : MonoBehaviour
    {
        public Animator animator;
        private static readonly int Idle = Animator.StringToHash("t_idle");
        private static readonly int Walk = Animator.StringToHash("t_walk");
        private static readonly int Dash = Animator.StringToHash("t_dash");
        private static readonly int Jump = Animator.StringToHash("t_jump");
        private static readonly int EndJump = Animator.StringToHash("t_endJump");
        private static readonly int WallJump = Animator.StringToHash("t_wallJump");
        private static readonly int WallSlide = Animator.StringToHash("t_wallSlide");
        private static readonly int Die = Animator.StringToHash("t_die");
        
        private void Awake()
        {
            EventBus.Subscribe<StartIdleEvent>(SetIdle);
            EventBus.Subscribe<StartWalkingEvent>(SetWalking);
            EventBus.Subscribe<StartJumpingEvent>(SetJumping);
            EventBus.Subscribe<StartWallJumpingEvent>(SetWallJumping);
            EventBus.Subscribe<StartWallSlidingEvent>(SetWallSliding);
            EventBus.Subscribe<StartDeadEvent>(SetDead);
            EventBus.Subscribe<StartDashEvent>(SetDashing);
            EventBus.Subscribe<StartEndJumpingEvent>(SetEndJumping);
            
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
        }
        
        private void SetIdle(StartIdleEvent args)
        {
            animator.SetTrigger(Idle);
        }
        
        private void SetWalking(StartWalkingEvent args)
        {
            animator.SetTrigger(Walk);
        }

        private void SetDashing(StartDashEvent args)
        {
            animator.SetTrigger(Dash);
        }

        private void SetJumping(StartJumpingEvent args)
        {
            animator.SetTrigger(Jump);
        }

        private void SetEndJumping(StartEndJumpingEvent args)
        {
            animator.SetTrigger(EndJump);
        }

        private void SetWallJumping(StartWallJumpingEvent args)
        {
            animator.SetTrigger(WallJump);
        }

        private void SetWallSliding(StartWallSlidingEvent args)
        {
            animator.SetTrigger(WallSlide);
        }

        private void SetDead(StartDeadEvent args)
        {
            animator.SetTrigger(Die);
        }
    }
}
