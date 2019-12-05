using UnityEngine;


[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    #region PrivateData
    [SerializeField] private Sprite _model;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _maxArmour;
    #endregion


    #region Field
    public string fullName;
    public Sprite portrait;
    #endregion


    #region Property
    public Sprite GetModel { get => _model; }
    public float GetMaxHealth { get => _maxHealth; }
    public float GetMaxArmour { get => _maxArmour; }
    #endregion
}
