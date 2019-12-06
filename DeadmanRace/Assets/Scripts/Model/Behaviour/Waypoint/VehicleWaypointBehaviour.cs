using UnityEngine;


namespace DeadmanRace
{
    [DisallowMultipleComponent]
    [AddComponentMenu("DeadmanRace/AI/Vehicle WaypointBehaviour", 1)]

    //Class for vehicle waypoints
    public class VehicleWaypointBehaviour : MonoBehaviour
    {
        #region PrivateData
        public VehicleWaypointBehaviour nextPoint;
        public float radius = 10;

        [Tooltip("Percentage of a vehicle's max speed to drive at")]
        [Range(0, 1)]
        public float speed = 1;
        #endregion


        #region UnityMethods
        void OnDrawGizmos()
        {
            //Visualize waypoint
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);

            //Draw line to next point
            if (nextPoint)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position, nextPoint.transform.position);
            }
        }
        #endregion
    }
}