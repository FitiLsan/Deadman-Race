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
            MyCharacter character = new MyCharacter(obj, playerData);

            _context.MyCharacter = character;
        }

        #endregion
    }
}
