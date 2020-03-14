using UnityEngine;


namespace DeadmanRace
{
    public sealed class InitializationPlayerController : IInitializeController
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycles

        public InitializationPlayerController(GameContext context, Services services)
        {
            _context = context;
        }

        #endregion


        #region IInitializeController

        public void Initialize()
        {
            var resources = Resources.Load<PlayerBehaviour>(AssetsPathGameObject.Object[GameObjectType.Character]);
            var playerData = Data.PlayerData;
            var obj = Object.FindObjectOfType<PlayerBehaviour>().transform;
            IWalk character;
            //MyCharacter character = new MyCharacter(obj, playerData);
            if (obj.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rgb))
            {
                character = new UnitMotor(obj, playerData);
            }
            else if (obj.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rgb2d))
            {
                character = new UnitMotor2D(obj, playerData);
            }
            else
            {
                obj.gameObject.AddComponent<Rigidbody>();
                character = new UnitMotor(obj, playerData);
            }
            _context.MyCharacter = character;
        }

        #endregion
    }
}
