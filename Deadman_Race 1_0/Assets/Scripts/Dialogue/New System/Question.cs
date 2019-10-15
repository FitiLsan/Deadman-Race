using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public struct Choice
{
    //можем выбрать текст
    [TextArea(2, 5)]
    public string text;
    //разговор
    public Conversation conversation;
}

[CreateAssetMenu(fileName = "New Question", menuName = "Question")]
public class Question : ScriptableObject
{
    [TextArea(2, 5)]
    public string text;
    public Choice[] choices;
}
