using DeadmanRace.Interfaces;
using DeadmanRace.Items;
using DeadmanRace.Enums;

namespace DeadmanRace.Components
{
    public sealed class CarEngine : BaseCarComponent<Engine>, ISetDamage<float>
    {
        public float CurentHealth { get; private set; }
        
        public float MaxHealth
        {
            get
            {
                if (_descriptionIsNull) return 0f;

                return _description.MaxHealth;
            }
        }

        public float GetTorque(CarFuelTank fuelSource, float traveledDistance)
        {
            var result = 0f;

            if (_descriptionIsNull) return result;

            switch (fuelSource.CurentFuelType)
            {
                case FuelTypes.Good:
                    result = _description.Power;
                    break;

                case FuelTypes.Medium:
                    result = _description.Power * _description.ReducePowerByFuel;
                    break;

                case FuelTypes.Bad:
                    result = _description.Power * _description.ReducePowerByFuel;
                    // damage
                    break;

                default:
                    result = 0;
                    break;
            }

            return result;
        }
        
        public void SetDamage(float damage)
        {
            CurentHealth -= damage;

            CurentHealth = CurentHealth <= 0f ? 0f : CurentHealth;
        }
    }
}