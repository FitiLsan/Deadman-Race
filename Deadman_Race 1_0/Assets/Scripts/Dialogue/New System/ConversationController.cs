using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
[System.Serializable]
public class QuestionEvent : UnityEvent<Question> { }

public class ConversationController : MonoBehaviour
{
    //ссылка на  скриптаблОбжект Conversation - разговор, с которого начинаем диалог
    //переносим Conversation с которого начнется диалог
    public Conversation conversation;
    public QuestionEvent questionEvent;
    //private bool conversationStarted;

    public GameObject speakerLeft;
    public GameObject speakerRight;

    private SpeakerUI speakerUILeft;
    private SpeakerUI speakerUIRight;

    private int activeLineIndex;
    private bool сonversationStarted = false;

    public void ChangeConversation(Conversation nextConversation)
    {
        сonversationStarted = false;
        conversation = nextConversation;
        AdvanceLine();
    }
    private void Start()
    {
        speakerUILeft = speakerLeft.GetComponent<SpeakerUI>();
        speakerUIRight = speakerRight.GetComponent<SpeakerUI>();
    }
    private void Update()
    {
        if (Input.GetKeyDown("space"))
            AdvanceLine();
        //пропустить диалог - начать  сцену
        else if (Input.GetKeyDown(KeyCode.Escape))
            EndConversation();
    }
    private void EndConversation()
    {

        conversation = null;
        сonversationStarted = false;
        speakerUILeft.Hide();
        speakerUIRight.Hide();
        SceneManager.LoadScene(1);
    }
    private void Initialize()
    {
        сonversationStarted = true;
        activeLineIndex = 0;
        speakerUILeft.Speaker = conversation.speakerLeft;
        speakerUIRight.Speaker = conversation.speakerRight;
    }
    private void AdvanceLine()
    {
        if (conversation == null) return;
        if (!сonversationStarted) Initialize();

        if (activeLineIndex < conversation.lines.Length)
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
        Line line = conversation.lines[activeLineIndex];
        Character character = line.character;

        if (speakerUILeft.SpeakerIs(character))
        {
            SetDialog(speakerUILeft, speakerUIRight, line.text);
        }
        else
        {
            SetDialog(speakerUIRight, speakerUILeft, line.text);
        }
        activeLineIndex += 1;
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
    /// показ диалогов
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
}
