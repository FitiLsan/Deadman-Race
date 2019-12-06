using UnityEngine;


namespace DeadmanRace
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
    public sealed class PlayerData : ScriptableObject
    {
        #region Fields
        //add for class UnitMotor for our player
        public float Velosity  = 5.0f;
        public Vector3 Position;

        #endregion
    }
}
