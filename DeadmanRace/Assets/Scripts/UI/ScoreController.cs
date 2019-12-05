using UnityEngine;


namespace DeadmanRace.UI
{
    public class ScoreController : MonoBehaviour
    {
        #region Field
        public int HitPoints;
        #endregion


        #region UnityMethods
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                HitPoints--;

                if (HitPoints <= 0)
                {
                    GameUIController.Instance.SlayScore();
                    Destroy(gameObject);
                }
            }
        }
        #endregion
    }
}

