using UnityEngine;

namespace DeadmanRace
{
    public interface IWalk
    {
        Transform Transform { get; set; }
        void Walk(float h, float v);
    }
}
