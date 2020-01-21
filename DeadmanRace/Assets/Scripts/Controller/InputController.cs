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
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var handbreak = Input.GetAxis("Jump");
            CustomDebug.Log(horizontal);
            CustomDebug.Log(vertical);

            if (!InCar) _context.MyCharacter.Walk(horizontal, vertical);
            else
            {
                _context.PlayerCar.Drive(horizontal, vertical, vertical, handbreak);
                _context.MyCharacter.Transform.position = _context.PlayerCar.CarTransform.position;
            }

            if (InCar)
            {
                _context.MyCharacter.Transform.gameObject.SetActive(false);
            }
            else
            {
                _context.MyCharacter.Transform.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                if (equipment == null) equipment = _context.CarEquipment;

                equipment.gameObject.SetActive(!equipment.gameObject.activeSelf);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if(Physics.Raycast(_context.MyCharacter.Transform.position, _context.MyCharacter.Transform.forward) && !InCar)
                {
                    InCar = !InCar;
                }
                else if (InCar)
                {
                    InCar = false;
                }
            }
        }

        #endregion
    }
}
