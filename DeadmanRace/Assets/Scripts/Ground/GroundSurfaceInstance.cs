using UnityEngine;

namespace DeadmanRace
{
    [RequireComponent(typeof(Collider))]
    [DisallowMultipleComponent]
    [AddComponentMenu("DeadmanRace/Ground Surface/Ground Surface Instance", 1)]

    //Class for instances of surface types
    public class GroundSurfaceInstance : MonoBehaviour
    {

        #region Field
        [Tooltip("Which surface type to use from the GroundSurfaceMaster list of surface types")]
        public int surfaceType;
        [System.NonSerialized]
        public float friction;
        #endregion


        #region UnityMethods
        private void Start()
        {
            //Set friction
            if (GroundSurfaceMaster.surfaceTypesStatic[surfaceType].UseColliderFriction)
            {
                friction = GetComponent<Collider>().material.dynamicFriction * 2;
            }
            else
            {
                friction = GroundSurfaceMaster.surfaceTypesStatic[surfaceType].Friction;
            }
        }
        #endregion
    }
}