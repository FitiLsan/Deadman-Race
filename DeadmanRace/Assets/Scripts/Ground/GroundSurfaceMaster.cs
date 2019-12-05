using UnityEngine;


namespace DeadmanRace
{
    [DisallowMultipleComponent]
    [AddComponentMenu("DeadmanRace/Ground Surface/Ground Surface Master", 0)]

    //Class managing surface types
    public class GroundSurfaceMaster : MonoBehaviour
    {
        #region Field
        public GroundSurface[] surfaceTypes;
        public static GroundSurface[] surfaceTypesStatic;
        #endregion


        #region UnityMethods
        void Start()
        {
            surfaceTypesStatic = surfaceTypes;
        }
        #endregion
    }

    //Class for individual surface types
    [System.Serializable]
    public class GroundSurface
    {
        #region Field
        public string Name = "Surface";
        public bool UseColliderFriction;
        public float Friction;
        [Tooltip("Always leave tire marks")]
        public bool AlwaysScrape;
        [Tooltip("Rims leave sparks on this surface")]
        public bool LeaveSparks;
        public AudioClip TireSound;
        
        #endregion
    }
}