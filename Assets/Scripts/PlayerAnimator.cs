using Mirror;
using UnityEngine;

namespace LeapGame
{
    public class PlayerAnimator : NetworkBehaviour
    {
        [SerializeField] Animator playerAnimation;

        private void Update()
        {
            if (isLocalPlayer == false)
                return;

            if (Input.GetKey(KeyCode.W))
            {
                SetAnimation(PlayerAnimation.run_forward);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                SetAnimation(PlayerAnimation.run_back);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                SetAnimation(PlayerAnimation.run_left);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                SetAnimation(PlayerAnimation.run_right);
            }
            else
            {
                SetAnimation(PlayerAnimation.idle);
            }
        }

        public void SetAnimation(PlayerAnimation newAnimation)
        {
            DisableAllAnimations();
            playerAnimation.SetBool(newAnimation.ToString(), true);
        }

        public void DisableAllAnimations()
        {
            playerAnimation.SetBool(PlayerAnimation.idle.ToString(), false);
            playerAnimation.SetBool(PlayerAnimation.run_left.ToString(), false);
            playerAnimation.SetBool(PlayerAnimation.run_right.ToString(), false);
            playerAnimation.SetBool(PlayerAnimation.run_forward.ToString(), false);
            playerAnimation.SetBool(PlayerAnimation.run_back.ToString(), false);
        }

        public enum PlayerAnimation
        {
            idle,

            run_forward,
            run_back,

            run_left,
            run_right
        }
    }
}