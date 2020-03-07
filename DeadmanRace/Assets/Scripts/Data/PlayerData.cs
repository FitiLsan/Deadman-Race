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

        public float MaxSpeed = 200.0f; // максимальная скорость движения
        public float RotationSpeed = 0.5f; // скорость поворота
        public float AccelerationForce = 20.0f; // сила ускорения
        public float BrakeForce = 40.0f; // сила торможения
        public float Clatch = 5.0f; // проскальзывание, данный параметр необходимо будет брать в зависимости от территории

        #endregion
    }
}
