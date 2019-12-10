using UnityEngine;
using DeadmanRace.Components;


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

        Equipment equipment;
        bool InCar = false;

        public void Execute()
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
            var j = Input.GetAxis("Jump");
            CustomDebug.Log(h);
            CustomDebug.Log(v);

            if (!InCar) _context.MyCharacter.Walk(h, v);
            else
            {
                _context.PlayerCar.Drive(h, v, v, j);
                _context.MyCharacter.Transform.position = _context.PlayerCar.CarTransform.position;
            }

            if (InCar)
            {
                _context.MyCharacter.Transform.gameObject.SetActive(!InCar);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                if (equipment == null) equipment = Object.FindObjectOfType<Equipment>();

                equipment.gameObject.SetActive(!equipment.gameObject.activeSelf);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if(Physics.Raycast(_context.MyCharacter.Transform.position, _context.MyCharacter.Transform.forward, 3f))
                {
                    InCar = !InCar;
                }
            }
        }

        #endregion
    }
}
