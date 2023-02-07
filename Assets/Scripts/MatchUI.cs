using UnityEngine;
using UnityEngine.UI;

namespace LeapGame
{
    public class MatchUI : MonoBehaviour
    {
        [SerializeField] Text myScore;
        [SerializeField] Text enemyScore;

        [SerializeField] Text myName;
        [SerializeField] Text enemyName;

        [SerializeField] Text winnerLine1;
        [SerializeField] Text winnerLine2;
        [SerializeField] GameObject winnerInfoBody;

        [SerializeField] GameObject matchUI_Body;

        private static MatchUI instance;

        public void Awake()
        {
            instance = this;
        }

        public Text MyScore { get => myScore; }
        public Text EnemyScore { get => enemyScore; }
        public Text MyName { get => myName; }
        public Text EnemyName { get => enemyName; }
        public Text WinnerLine1 { get => winnerLine1; set => winnerLine1 = value; }
        public Text WinnerLine2 { get => winnerLine2; set => winnerLine2 = value; }
        public GameObject WinnerInfoBody { get => winnerInfoBody; }
        public static MatchUI Instance { get { return instance; } }
        public GameObject MatchUI_Body { get => matchUI_Body; set => matchUI_Body = value; }
    }
}