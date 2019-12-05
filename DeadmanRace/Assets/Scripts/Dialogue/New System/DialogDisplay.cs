using UnityEngine;


namespace DeadmanRace
{
    public class DialogDisplay : MonoBehaviour
    {
        #region PrivateData
        private SpeakerUI _speakerUILeft;
        private SpeakerUI _speakerUIRight;
        private int _activeLineIndex = 0;
        #endregion


        #region Field
        public Conversation conversation;
        public GameObject speakerLeft;
        public GameObject speakerRight;
        #endregion


        #region UnityMethods
        private void Start()
        {
            _speakerUILeft = speakerLeft.GetComponent<SpeakerUI>();
            _speakerUIRight = speakerRight.GetComponent<SpeakerUI>();

            _speakerUILeft.Speaker = conversation.speakerLeft;
            _speakerUIRight.Speaker = conversation.speakerRight;
        }

        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                AdvanceConversation();
            }

        }
        #endregion


        #region Methods
        private void AdvanceConversation()
        {
            if (_activeLineIndex < conversation.lines.Length)
            {
                DisplayLine();
                _activeLineIndex += 1;
            }
            else
            {
                _speakerUILeft.Hide();
                _speakerUIRight.Hide();
                _activeLineIndex = 0;
            }
        }

        private void DisplayLine()
        {
            Line line = conversation.lines[_activeLineIndex];
            Character character = line.character;

            if (_speakerUILeft.SpeakerIs(character))
            {
                SetDialog(_speakerUILeft, _speakerUIRight, line.text);
            }
            else
            {
                SetDialog(_speakerUIRight, _speakerUILeft, line.text);
            }
        }

        private void SetDialog(
            SpeakerUI activeSpeakerUI,
            SpeakerUI inactiveSpeakerUI,
            string text
        )
        {
            activeSpeakerUI.Dialog = text;
            activeSpeakerUI.Show();
            inactiveSpeakerUI.Hide();
        }
        #endregion
    }
}

