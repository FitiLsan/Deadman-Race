using UnityEngine;
using UnityEngine.UI;


namespace DeadmanRace.UI
{
    public class IconUI : MonoBehaviour
    {
        #region Fields

        private Image _icon;
        private bool _iconIsNull = true;

        #endregion


        #region Properties

        public Sprite GetIcon { get => _icon.sprite; }

        public bool Enabled { get => _icon.enabled; set => _icon.enabled = value; }

        #endregion


        #region UnityMethods

        private void Awake() => _iconIsNull = !TryGetComponent(out _icon);

        #endregion


        #region Methods

        public void Set(Sprite iconSprite)
        {
            if (_iconIsNull) return;

            _icon.sprite = iconSprite;

            _icon.enabled = true;
        }

        public void Clear()
        {
            if (_iconIsNull) return;

            _icon.enabled = false;

            _icon.sprite = null;
        }

        #endregion
    }
}
