using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace DeadmanRace.UI
{
    public class TimerController : MonoBehaviour
    {
        #region PrivateData
        private TimeSpan _timePlaying;
        private bool _timerGoing;
        private float _elapsedTime;
        private string _uiText = "Time: 00:00.00";
        #endregion


        #region Field
        public static TimerController Instance;
        public Text timeCounter;
        #endregion


        #region UnityMethods
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            timeCounter.text = _uiText;
            _timerGoing = false;
        }
        #endregion


        #region Methods
        public void BeginTimer()
        {
            _timerGoing = true;
            _elapsedTime = 0f;

            StartCoroutine(UpdateTimer());
        }

        public void EndTimer()
        {
            _timerGoing = false;
        }

        private IEnumerator UpdateTimer()
        {
            while (_timerGoing)
            {
                _elapsedTime += Time.deltaTime;
                _timePlaying = TimeSpan.FromSeconds(_elapsedTime);
                string timePlaying = "Time: " + _timePlaying.ToString("mm':'ss'.'ff");
                timeCounter.text = timePlaying;

                yield return null;
            }
        }
        #endregion
    }
}

