using System;

namespace DeadmanRace.Interfaces
{
    public interface IUpdateComponent
    {
        event Action OnFixedUpdate;
        event Action OnLateUpdate;
        event Action OnUpdate;
    }
}