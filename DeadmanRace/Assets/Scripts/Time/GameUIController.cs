using System;
using UnityEngine;
using UnityEngine.UI;


namespace DeadmanRace.UI
{
    public class GameUIController : MonoBehaviour
    {
        #region PrivateData
        private int _numberTotalKills;
        private int _numberSlayedKills;
        private TimeSpan _timePlaying;
        #endregion


        #region Field
        public static GameUIController Instance;
        public GameObject HudContainer;
        public GameObject KillScoreContainer;
        public GameObject GameOverPanel;
        public Text KillScoreCounter;
        #endregion


        #region Properties

        public bool gamePlaying { get; private set; }
        #endregion


        #region UnityMethods
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _numberTotalKills = KillScoreContainer.transform.childCount;
            _numberSlayedKills = 0;
            KillScoreCounter.text = "Score: 0 / " + _numberTotalKills;

            gamePlaying = false;
        }

        #endregion


        #region Methods
        public void SlayScore()
        {
            _numberSlayedKills++;
            string demonCount = "Score: " + _numberSlayedKills + " / " + _numberTotalKills;
            KillScoreCounter.text = demonCount;

            if (_numberSlayedKills >= _numberTotalKills)
            {
                EndGame();
            }
        }

        private void EndGame()
        {
            gamePlaying = false;
            Invoke("ShowGameOverScreen", 1.25f);
        }

        private void ShowGameOverScreen()
        {
            GameOverPanel.SetActive(true);
            HudContainer.SetActive(false);
            string timePlaying = "Time: " + _timePlaying.ToString("mm':'ss'.'ff");
            GameOverPanel.transform.Find("FinalTimeText").GetComponent<Text>().text = timePlaying;
        }

        public void BeginGame()
        {
            gamePlaying = true;
            TimerController.Instance.BeginTimer();
        }
        #endregion
    }
}

