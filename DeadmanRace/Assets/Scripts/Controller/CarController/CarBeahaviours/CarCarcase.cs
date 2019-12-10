using DeadmanRace.Items;

namespace DeadmanRace.Components
{
    public sealed class CarCarcase : BaseCarComponent<Carcase>
    {
        public float MaxHealth { get; private set; }

        public float CurentHealth { get; private set; }
    }
}