using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DeadmanRace
{
    public class QuestionController : MonoBehaviour
    {
        #region PrivateData
        private List<ChoiceController> _choiceControllers = new List<ChoiceController>();
        #endregion


        #region Fields
        public Question question;
        public Text questionText;
        public Button choiceButton;
        #endregion


        #region Methods
        public void Change(Question _question)
        {
            RemoveChoices();
            question = _question;
            gameObject.SetActive(true);
            Initialize();
        }

        public void Hide(Conversation conversation)
        {
            RemoveChoices();
            gameObject.SetActive(false);
        }

        private void RemoveChoices()
        {
            foreach (ChoiceController c in _choiceControllers)
                Destroy(c.gameObject);
            _choiceControllers.Clear();
        }

        private void Initialize()
        {
            questionText.text = question.text;
            for (int index = 0; index < question.choices.Length; index++)
            {
                ChoiceController c = ChoiceController.AddChoiceButton(choiceButton, question.choices[index], index);
                _choiceControllers.Add(c);
            }
            choiceButton.gameObject.SetActive(false);
        }
        #endregion
    }
}

