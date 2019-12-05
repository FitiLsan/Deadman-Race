using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace DeadmanRace
{
    [System.Serializable]
    public class ConversationChangeEvent : UnityEvent<Conversation> { }

    public class ChoiceController : MonoBehaviour
    {
        #region Field
        public Choice choice;
        //dialogue change event link
        public ConversationChangeEvent conversationChangeEvent;
        #endregion


        #region UnityMethods
        private void Start()
        {
            if (conversationChangeEvent == null)
            {
                conversationChangeEvent = new ConversationChangeEvent();
            }
            GetComponent<Button>().GetComponentInChildren<Text>().text = choice.text;
        }
        #endregion


        #region Methods
        /// <summary>
        /// creation of buttons depending on the number of Choice in ScriptableObjecte Question
        /// </summary>
        /// <param name="choiceButtonTemplate"></param>
        /// <param name="choice"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static ChoiceController AddChoiceButton(Button choiceButtonTemplate, Choice choice, int index)
        {
            //distance between buttons
            int buttonSpacing = -44;
            Button button = Instantiate(choiceButtonTemplate);
            //create parent folder (container)
            button.transform.SetParent(choiceButtonTemplate.transform.parent);
            button.transform.localScale = Vector3.one;
            // set the button position(each one is created below the previous button depending on the Index value)
            button.transform.localPosition = new Vector3(0, index * buttonSpacing, 0);
            button.name = "Choice" + (index + 1);
            button.gameObject.SetActive(true);
            ChoiceController choiceController = button.GetComponent<ChoiceController>();
            //the selection button will be able to make a choice among the saved options
            choiceController.choice = choice;
            return choiceController;
        }

        //hangs on the Choice Button in the Button component - On Click
        public void MakeChoice()
        {
            conversationChangeEvent.Invoke(choice.conversation);
        }
        #endregion
    }
}
