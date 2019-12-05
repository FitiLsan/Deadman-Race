namespace DeadmanRace
{
    public enum UpdateType 
    {
        Fixed    = 0,
        Update   = 1,
        Late     = 2,
    #if UNITY_EDITOR
        Gizmos   = 3
    #endif
    }
}


