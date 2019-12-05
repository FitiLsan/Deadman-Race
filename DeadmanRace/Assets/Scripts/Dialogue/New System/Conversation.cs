using UnityEngine;


namespace DeadmanRace
{
    [System.Serializable]
    public struct Line
    {
        #region Field
        public Character character;

        [TextArea(2, 5)]
        public string text;
        #endregion
    }

    [CreateAssetMenu(fileName = "New Conversation", menuName = "Conversation/Intros")]
    public class Conversation : ScriptableObject
    {
        #region Field
        public Character speakerLeft;
        public Character speakerRight;
        public Line[] lines;
        public Question question;
        public Conversation nextConversation;
        #endregion
    }

}
