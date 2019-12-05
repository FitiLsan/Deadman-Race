using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace DeadmanRace.UI
{
    public class CountDownController : MonoBehaviour
    {
        #region PrivateData
        private AudioSource _startTick;
        #endregion


        #region Field
        public int CountdownTime = 3;
        public Text CountdownDisplay;
        //public AudioClip AudioClip;
        #endregion


        #region UnityMethods
        private void Start()
        {
            //AudioClip = Resources.Load<AudioClip>("Audio\\");
            StartCoroutine(CountdownToStart());
        }
        #endregion


        #region Methods
        private IEnumerator CountdownToStart()
        {
            while (CountdownTime > 0)
            {
                CountdownDisplay.text = CountdownTime.ToString();

                yield return new WaitForSeconds(1.0f);
                CountdownTime--;
                //_startTick.PlayOneShot(AudioClip);
            }
            CountdownDisplay.text = "GO !";
            GameUIController.Instance.BeginGame();
            yield return new WaitForSeconds(1.0f);
            CountdownDisplay.gameObject.SetActive(false);
        }
        #endregion
    }
}

