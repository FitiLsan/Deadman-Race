using UnityEngine;


namespace DeadmanRace
{
    public sealed class InputController : IExecuteController
    {
        #region Fields
        
        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycles
        
        public InputController(GameContext context, Services services)
        {
            _context = context;
        }

        #endregion


        #region IExecuteController
        
        public void Execute()
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
            Debug.Log("OnWork");
            Debug.Log(h);
            Debug.Log(v);
            _context.MyCharacter.Walk(h, v);

        }

        #endregion
    }
}
