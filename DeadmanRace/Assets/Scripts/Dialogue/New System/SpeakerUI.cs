using UnityEngine;
using UnityEngine.UI;


namespace DeadmanRace
{
    public class SpeakerUI : MonoBehaviour
    {
        #region PrivateData
        private Character _speaker;
        #endregion


        #region Field
        public Image portrait;
        public Text fullName;
        public Text dialog;
        #endregion


        #region Property
        public Character Speaker
        {
            get { return _speaker; }
            set
            {
                _speaker = value;
                portrait.sprite = _speaker.portrait;
                fullName.text = _speaker.fullName;
            }
        }

        public string Dialog
        {
            set { dialog.text = value; }
        }
        #endregion


        #region Methods
        public bool HasSpeaker()
        {
            return _speaker != null;
        }

        public bool SpeakerIs(Character character)
        {
            return _speaker == character;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}

