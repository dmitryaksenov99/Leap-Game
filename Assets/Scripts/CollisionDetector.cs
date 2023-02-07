using Mirror;
using UnityEngine;

namespace LeapGame
{
    public class CollisionDetector : NetworkBehaviour
    {
        [SerializeField] Player player;
        [SerializeField] CapsuleCollider triggerCollider;

        private void OnTriggerEnter(Collider collider)
        {
            if (gameObject == collider.gameObject)
                return;

            if (collider.gameObject.CompareTag("Player") == false)
                return;

            if (player.PlayerMover.IsLeap == false)
                return;

            OnCollision(collider);
        }

        public void OnCollision(Collider collider)
        {
            Player enemyPlayer = collider.gameObject.GetComponent<Player>();

            if (player.PlayerMover.Velocity < enemyPlayer.PlayerMover.Velocity)
                return;

            if (enemyPlayer.IsPlayerInvulnerable)
                return;

            enemyPlayer.IsPlayerInvulnerable = true;

            CmdOnCollision(enemyPlayer);
        }

        [Command]
        public void CmdOnCollision(Player enemyPlayer)
        {
            RpcOnCollision(enemyPlayer);
        }

        [ClientRpc]
        private void RpcOnCollision(Player enemyPlayer)
        {
            Physics.IgnoreCollision(
                  player.PlayerMover.CharacterController,
                  enemyPlayer.PlayerMover.CharacterController);

            StartCoroutine(enemyPlayer.MakeDamaged(player));
        }
    }
}