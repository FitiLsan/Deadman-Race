using DeadmanRace.Items;
using DeadmanRace.Enums;


namespace DeadmanRace.Components
{
    public sealed class CarEngine : BaseCarComponent<Engine>, ISetDamage<float>
    {
        #region Properties

        public float CurentHealth { get; private set; }

        public float MaxHealth
        {
            get
            {
                if (_descriptionIsNull) return 0f;

                return _description.MaxHealth;
            }
        }

        #endregion


        #region Methods

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
                    break;

                default:
                    result = 0f;
                    break;
            }

            return result;
        }

        #endregion


        #region ISetDamage

        public void SetDamage(float damage)
        {
            if (_descriptionIsNull) return;

            CurentHealth -= damage;
            CurentHealth = CurentHealth <= 0f ? 0f : CurentHealth;
        }

        #endregion
    }
}