using System;
using System.Collections.Generic;
using UnityEngine;
using DeadmanRace.Interfaces;
using DeadmanRace.Enums;
using DeadmanRace.Objects;
using DeadmanRace.UI;

namespace DeadmanRace.Components
{

    public sealed class Equipment : MonoBehaviour, IEquipment
    {
        #region Fields

        [SerializeField]
        private ObjectType _targetObjectType;

        private SlotUI[] _slotsUI;

        private IEquipmentSlot[] _equipment;

        private IEquipableComponent[] _equipableObjects;
        private bool _equipableObjectTransformIsNull = true;

        #endregion


        #region Properties

        public ObjectType GetTargetObjecType { get => _targetObjectType; }

        #endregion


        #region UnityMethods

        private void Awake()
        {
            _slotsUI = GetComponentsInChildren<SlotUI>();
            if (_slotsUI.Length == 0) throw new NullReferenceException("[Equipment] UI slots not found");

            _equipment = new IEquipmentSlot[_slotsUI.Length];

            for (int i = 0; i < _slotsUI.Length; i++)
            {
                var slot = new EquipmentSlot(_slotsUI[i].GetSlotType);
                slot.IsActive = false;
                
                _equipment[i] = slot;
                _slotsUI[i].AttachEquipmentSlot(slot);
            }
        }

        private void OnEnable()
        {
            for (int i = 0; i < _slotsUI.Length; i++)
            {
                _equipment[i].OnChange += _slotsUI[i].UpdateSlot;
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _slotsUI.Length; i++)
            {
                _equipment[i].OnChange -= _slotsUI[i].UpdateSlot;
            }
        }

        #endregion


        #region IEquipment

        public void AttachObject(Transform objTransform)
        {
            if (!_equipableObjectTransformIsNull)
            {
                for (int i = 0; i < _equipableObjects.Length; i++)
                {
                    _equipableObjects[i].UnattachSlot();
                }
            }

            _equipableObjects = objTransform.GetComponentsInChildren<IEquipableComponent>();
            if (_equipableObjects != null) _equipableObjectTransformIsNull = false;

            if (!_equipableObjectTransformIsNull)
            {
                for (int i = 0; i < _equipment.Length; i++)
                {
                    for (int j = 0; j < _equipableObjects.Length; j++)
                    {
                        if (_equipment[i].Type == _equipableObjects[j]._componentType)
                        {
                            if (_equipableObjects[j].AttachSlot(_equipment[i])) break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}