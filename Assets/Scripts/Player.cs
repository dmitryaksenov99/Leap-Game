using Mirror;
using System.Collections;
using UnityEngine;

namespace LeapGame
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] PlayerMover playerMover;
        [SerializeField] UI_Updater uiUpdater;
        [SerializeField] Renderer playerMeshRenderer;

        [SerializeField] float colorChangeTimeout = 3f;
        [SerializeField] float scoreLimit = 3f;
        [SerializeField] float matchRestartTimeout = 5f;

        [SerializeField] Color normalColor = Color.white;
        [SerializeField] Color damagedColor = Color.red;

        private string playerName;
        private int playerScore;
        private bool isPlayerInvulnerable;

        public override void OnStartClient()
        {
            playerName = "Player " + netId;
        }

        [ClientRpc]
        public void RpcOnMatchStart(Player myName, Player enemyName)
        {
            playerMover.CanMove = true;

            CursorStateChanger.Lock();
            CursorStateChanger.Hide();

            GetComponent<NetworkRoomPlayer>().showRoomGUI = false;

            uiUpdater.EnableMatchUI();
            uiUpdater.CmdSetPlayersNames(myName, enemyName);
        }

        [ClientRpc]
        public void Respawn(Vector3 position, Quaternion rotation)
        {
            SetPlayerColor(normalColor);

            playerScore = 0;
            isPlayerInvulnerable = false;

            uiUpdater.ResetPlayersScoreUI();
            playerMover.ResetTransform(position, rotation);
        }

        public IEnumerator MakeDamaged(Player attackingPlayer)
        {
            isPlayerInvulnerable = true;

            attackingPlayer.PlayerScore++;
            uiUpdater.CmdSetScoreText(attackingPlayer);

            SetPlayerColor(damagedColor);

            if (attackingPlayer.PlayerScore >= scoreLimit)
            {
                StartCoroutine(EndMatchRoutine(attackingPlayer));
            }

            yield return new WaitForSeconds(colorChangeTimeout);
            SetPlayerColor(normalColor);

            isPlayerInvulnerable = false;
        }

        private IEnumerator EndMatchRoutine(Player attackingPlayer)
        {
            uiUpdater.ShowWinner(attackingPlayer.PlayerName, attackingPlayer.PlayerScore);

            yield return new WaitForSeconds(matchRestartTimeout);

            uiUpdater.HideWinner();

            if (isServer)
            {
                Room.Instance.RespawnPlayers();
            }
        }

        private void SetPlayerColor(Color color)
        {
            playerMeshRenderer.material.color = color;
        }

        public int PlayerScore { get => playerScore; set => playerScore = value; }
        public string PlayerName { get => playerName; set => playerName = value; }
        public PlayerMover PlayerMover { get => playerMover; }
        public UI_Updater UI_Updater { get => uiUpdater; }
        public bool IsPlayerInvulnerable { get => isPlayerInvulnerable; set => isPlayerInvulnerable = value; }
    }
}