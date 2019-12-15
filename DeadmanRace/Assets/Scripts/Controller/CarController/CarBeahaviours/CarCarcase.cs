using DeadmanRace.Items;


namespace DeadmanRace.Components
{
    public sealed class CarCarcase : BaseCarComponent<Carcase>, ISetDamage<float>
    {
        #region Properties

        public float MaxHealth { get; private set; }

        public float CurentHealth { get; private set; }

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