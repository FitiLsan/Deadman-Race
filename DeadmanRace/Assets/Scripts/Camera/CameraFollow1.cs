using UnityEngine;

namespace DeadmanRace
{
    public class CameraFollow1 : MonoBehaviour
    {
        #region PrivateData
        private Rigidbody _carRigidbody;
        private Camera _mainCamera;
        #endregion


        #region Field
        public float xMargin = 1f;      // Distance in the x axis the player can move before the camera follows.
        public float yMargin = 1f;      // Distance in the y axis the player can move before the camera follows.
        public float xSmooth = 8f;      // How smoothly the camera catches up with it's target movement in the x axis.
        public float ySmooth = 8f;      // How smoothly the camera catches up with it's target movement in the y axis.
        public Vector2 maxXAndY;        // The maximum x and y coordinates the camera can have.
        public Vector2 minXAndY;        // The minimum x and y coordinates the camera can have.
        public float minCameraDist = 30f;      
        public float maxCameraDist = 50f;

        public Transform Player;        // Reference to the player's transform.
        public Transform Car;
        #endregion


        #region UnityMethods
        void Awake()
        {
            // Setting up the reference.
            Player = GameObject.FindGameObjectWithTag("Player").transform;
           
            _mainCamera = gameObject.GetComponent<Camera>();
            if (_mainCamera == null) throw new System.NullReferenceException("Camera not found");
        }

        void FixedUpdate()
        {
            TrackPlayer();
        }
        #endregion


        #region Methods
        bool CheckXMargin()
        {
            // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
            return Mathf.Abs(transform.position.x - Player.position.x) > xMargin;
        }


        bool CheckYMargin()
        {
            // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
            return Mathf.Abs(transform.position.y - Player.position.y) > yMargin;
        }

        void TrackPlayer()
        {
            // Set the camera's position to the target position with the same z component.
            transform.position = new Vector3(Player.position.x, transform.position.y, Player.position.z);
        }
        #endregion
    }
}
