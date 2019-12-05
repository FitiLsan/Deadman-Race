using UnityEngine;


namespace DeadmanRace
{
    [System.Serializable]
    public struct Choice
    {
        #region Field
        [TextArea(2, 5)]
        public string text;
        public Conversation conversation;
        #endregion
    }

    [CreateAssetMenu(fileName = "New Question", menuName = "Question")]
    public class Question : ScriptableObject
    {
        #region Field
        [TextArea(2, 5)]
        public string text;
        public Choice[] choices;
        #endregion
    }
}

