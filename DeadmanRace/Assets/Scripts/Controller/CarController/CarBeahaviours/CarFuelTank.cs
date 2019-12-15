using DeadmanRace.Items;
using DeadmanRace.Enums;


namespace DeadmanRace.Components
{
    public sealed class CarFuelTank : BaseCarComponent<FuelTank>, ISetDamage<float>
    {
        #region Properties

        public float CurentHealth { get; private set; }

        public float MaxHealth { get; private set; }

        public float CurentFuelValue { get; private set; }

        public FuelTypes CurentFuelType { get; private set; } = FuelTypes.Good;

        #endregion


        #region Methods

        public override float GetWeight() => 
            _descriptionIsNull ? 0f : _description.Weight * (CurentFuelValue / _description.Capacity);
        
        public float GetFuel(float amount, out FuelTypes type)
        {
            if (_descriptionIsNull)
            {
                type = FuelTypes.Empty;
                return 0f;
            }

            type = CurentFuelType;

            if (CurentFuelValue > amount)
            {
                CurentFuelValue -= amount;

                return amount;
            }

            var valueToReturn = CurentFuelValue;

            CurentFuelValue = 0f;

            return valueToReturn;
        }

        public void AddFuel(float amount, FuelTypes type)
        {
            if (_descriptionIsNull) return;

            CurentFuelType = (int)CurentFuelType > (int)type ? type : CurentFuelType;

            CurentFuelValue += amount;

            CurentFuelValue = CurentFuelValue > _description.Capacity ? _description.Capacity : CurentFuelValue;
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