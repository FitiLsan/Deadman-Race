using UnityEngine;
using UnityEngine.UI;
using DeadmanRace.Interfaces;

namespace DeadmanRace.UI
{
    public class OutlineComponent : MonoBehaviour, IOutlineComponent
    {
        #region Fields

        private Image _outlineImage;
        private bool _outlineImageIsNull = true;

        #endregion


        #region UnityMethods

        public void Awake() => _outlineImageIsNull = !TryGetComponent(out _outlineImage);

        #endregion


        #region IOutlineComponent
        
        public void Enable()
        {
            if (_outlineImageIsNull) return;

            _outlineImage.enabled = true;
        }

        public void Disable()
        {
            if (_outlineImageIsNull) return;
            
            _outlineImage.enabled = false;
        }

        #endregion
    }
}
