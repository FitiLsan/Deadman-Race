using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace DeadmanRace.UI
{
    public class NewRaceCountdown : MonoBehaviour
    {

        #region Field
        public int CountdownTime = 30;
        public Text NewRaceCountdownDisplay;
        #endregion


        #region UnityMethods
        private void Start()
        {
            StartCoroutine(CountdownToStart());
        }
        #endregion


        #region Methods
        private IEnumerator CountdownToStart()
        {
            while (CountdownTime > 0)
            {
                NewRaceCountdownDisplay.text = CountdownTime.ToString();

                yield return new WaitForSeconds(1.0f);
                CountdownTime--;
            }
            NewRaceCountdownDisplay.text = "You did not have time to start a new race !";
            //GameController.Instance.NewRace();
            yield return new WaitForSeconds(1.0f);
            NewRaceCountdownDisplay.gameObject.SetActive(false);
        }
        #endregion
    }
}
