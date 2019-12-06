using UnityEngine;


namespace DeadmanRace
{
    public sealed class MyCharacter : IWalk
    {
        #region PrivateData
        private Vector3 _direction;
        private readonly float _velosity;
        private Rigidbody Rigidbody { get; }
        private PlayerBehaviour PlayerBehaviour { get; }
        #endregion


        #region ClassLifeCycles
        public MyCharacter(Transform transform, PlayerData playerData)
        {
            _direction = Vector2.zero;
            _velosity = playerData.Velosity;
            Transform = transform;
            Rigidbody = Transform.GetComponent<Rigidbody>();
            if (!Rigidbody) return;
            PlayerBehaviour = Transform.GetComponent<PlayerBehaviour>();
        }
        #endregion


        #region interface IWalk
        public void Walk(float hAxis, float vAxis)
        {
            CharecterMove(hAxis, vAxis);
            LookRotation();
        }
        public Transform Transform { get; set; }
        #endregion


        #region Methods
        private void CharecterMove(float hAxis, float vAxis)
        {
            _direction = new Vector3()
            {
                x = hAxis * _velosity,
                z = vAxis * _velosity,
                y = 0f
            };
            if (hAxis != 0f || vAxis != 0f)
            {
                Rigidbody.MovePosition(Rigidbody.position + _direction * Time.fixedDeltaTime);
            }
            else
            {
                Rigidbody.velocity = _direction;
                Rigidbody.angularVelocity = _direction;
            }
        }

        private void LookRotation()
        {
            Vector3 MousePos = Input.mousePosition;
            //Convert the player’s coordinates from world to screen
            Vector3 CharacterPos = Camera.main.WorldToScreenPoint(Transform.position);
            var VectorToTarget = MousePos - CharacterPos;
            float angleToTarget = Mathf.Atan2(VectorToTarget.x, VectorToTarget.y) * Mathf.Rad2Deg;
            Transform.rotation = Quaternion.Euler(0, angleToTarget, 0);
        }
        #endregion
    }
}
