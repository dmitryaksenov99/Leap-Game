using Mirror;
using UnityEngine;

namespace LeapGame
{
    public class UI_Updater : NetworkBehaviour
    {
        [SerializeField] Player player;
        [SerializeField] MatchUI matchUI;

        private bool cursorStateChanged;

        public void Awake()
        {
            matchUI = MatchUI.Instance;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (cursorStateChanged)
                {
                    cursorStateChanged = false;

                    CursorStateChanger.Lock();
                    CursorStateChanger.Hide();
                }
                else
                {
                    cursorStateChanged = true;

                    CursorStateChanger.Unlock();
                    CursorStateChanger.Show();
                }
            }
        }

        public void EnableMatchUI()
        {
            matchUI.MatchUI_Body.SetActive(true);
        }

        [TargetRpc]
        public void SetPlayersName(string myName, string enemyName)
        {
            matchUI.MyName.text = myName;
            matchUI.EnemyName.text = enemyName;
        }

        [Command]
        public void CmdSetScoreText(Player attackingPlayer)
        {
            player.UI_Updater.RpcSetScoreText(player.PlayerScore, attackingPlayer.PlayerScore);
            attackingPlayer.UI_Updater.RpcSetScoreText(attackingPlayer.PlayerScore, player.PlayerScore);
        }

        [Command]
        public void CmdSetPlayersNames(Player myName, Player enemyName)
        {
            myName.UI_Updater.SetPlayersName(myName.PlayerName, enemyName.PlayerName);
            enemyName.UI_Updater.SetPlayersName(enemyName.PlayerName, myName.PlayerName);
        }

        [TargetRpc]
        public void RpcSetScoreText(int myScore, int enemyScore)
        {
            matchUI.MyScore.text = myScore.ToString();
            matchUI.EnemyScore.text = enemyScore.ToString();
        }

        public void ShowWinner(string name, int score)
        {
            matchUI.WinnerLine1.text = $"{name} is win";
            matchUI.WinnerLine2.text = $"with score {score}";
            matchUI.WinnerInfoBody.SetActive(true);
        }

        public void HideWinner()
        {
            matchUI.WinnerInfoBody.SetActive(false);
            matchUI.WinnerLine1.text = "";
            matchUI.WinnerLine2.text = "";
        }

        public void ResetPlayersScoreUI()
        {
            string zero = "0";
            matchUI.MyScore.text = zero;
            matchUI.EnemyScore.text = zero;
        }
    }
}