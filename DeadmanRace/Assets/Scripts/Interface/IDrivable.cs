namespace DeadmanRace.Interfaces
{
    public interface IDrivable
    {
        void Drive(float steering, float accel, float footbrake, float handbrake);
    }
}