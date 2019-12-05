using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


namespace DeadmanRace
{
    [System.Serializable]
    public class QuestionEvent : UnityEvent<Question> { }

    public class ConversationController : MonoBehaviour
    {
        #region PrivateData
        private SpeakerUI _speakerUILeft;
        private SpeakerUI _speakerUIRight;
        private int _activeLineIndex;
        private bool _сonversationStarted = false;
        #endregion


        #region Field
        public Conversation conversation;
        public QuestionEvent questionEvent;
        public GameObject speakerLeft;
        public GameObject speakerRight;
        #endregion


        #region UnityMethods
        private void Start()
        {
            _speakerUILeft = speakerLeft.GetComponent<SpeakerUI>();
            _speakerUIRight = speakerRight.GetComponent<SpeakerUI>();
        }

        private void Update()
        {
            if (Input.GetKeyDown("space"))
                AdvanceLine();
            //пропустить диалог - начать  сцену
            else if (Input.GetKeyDown(KeyCode.Escape))
                EndConversation();
        }
        #endregion


        #region Methods
        public void ChangeConversation(Conversation nextConversation)
        {
            _сonversationStarted = false;
            conversation = nextConversation;
            AdvanceLine();
        }
        
        private void EndConversation()
        {

            conversation = null;
            _сonversationStarted = false;
            _speakerUILeft.Hide();
            _speakerUIRight.Hide();
            SceneManager.LoadScene(1);
        }

        private void Initialize()
        {
            _сonversationStarted = true;
            _activeLineIndex = 0;
            _speakerUILeft.Speaker = conversation.speakerLeft;
            _speakerUIRight.Speaker = conversation.speakerRight;
        }

        private void AdvanceLine()
        {
            if (conversation == null) return;
            if (!_сonversationStarted) Initialize();

            if (_activeLineIndex < conversation.lines.Length)
            {
                DisplayLine();
            }
            else
            {
                AdvanceConversation();
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
            _activeLineIndex += 1;
        }

        private void AdvanceConversation()
        {
            if (conversation.question != null)
            {
                questionEvent.Invoke(conversation.question);
            }
            else if (conversation.nextConversation != null)
            {
                ChangeConversation(conversation.nextConversation);
            }
            else
            {
                EndConversation();
            }
        }

        /// <summary>
        /// display dialogs
        /// </summary>
        /// <param name="activeSpeakerUI"></param>
        /// <param name="inactiveSpeakerUI"></param>
        /// <param name="text"></param>
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

