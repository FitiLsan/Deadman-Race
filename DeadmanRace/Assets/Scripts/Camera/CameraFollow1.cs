using UnityEngine;

namespace DeadmanRace
{
    public class CameraFollow1 : MonoBehaviour
    {
        #region PrivateData
        //private Rigidbody _carRigidbody;
        private Camera _mainCamera;
        #endregion


        #region Fields
        public float XMargin = 1.0f;      // Distance in the x axis the player can move before the camera follows.
        public float YMargin = 1.0f;      // Distance in the y axis the player can move before the camera follows.
        public float XSmooth = 8.0f;      // How smoothly the camera catches up with it's target movement in the x axis.
        public float YSmooth = 8.0f;      // How smoothly the camera catches up with it's target movement in the y axis.
        public Vector2 MaxXAndY;        // The maximum x and y coordinates the camera can have.
        public Vector2 MinXAndY;        // The minimum x and y coordinates the camera can have.
        public float MinCameraDist = 30.0f;      
        public float MaxCameraDist = 50.0f;

        public Transform Player;        // Reference to the player's transform.
        public Transform Car;
        #endregion


        #region UnityMethods
        private void Awake()
        {
            // Setting up the reference.
            Player = GameObject.FindGameObjectWithTag(TagManager.PLAYER).transform;
           
            _mainCamera = gameObject.GetComponent<Camera>();
            if (_mainCamera == null) throw new System.NullReferenceException("Camera not found");
        }

        private void FixedUpdate()
        {
            TrackPlayer();
        }
        #endregion


        #region Methods
        bool CheckXMargin()
        {
            // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
            return Mathf.Abs(transform.position.x - Player.position.x) > XMargin;
        }


        bool CheckYMargin()
        {
            // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
            return Mathf.Abs(transform.position.y - Player.position.y) > YMargin;
        }

        void TrackPlayer()
        {
            // Set the camera's position to the target position with the same z component.
            transform.position = new Vector3(Player.position.x, transform.position.y, Player.position.z);
        }
        #endregion
    }
}
