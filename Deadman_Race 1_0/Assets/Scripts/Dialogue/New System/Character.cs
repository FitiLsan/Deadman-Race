
using UnityEngine;
[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public string fullName;
    public Sprite portrait;

    [SerializeField] private Sprite _model;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _maxArmour;
    

    public Sprite GetModel { get => _model; }
    public float GetMaxHealth { get => _maxHealth; }
    public float GetMaxArmour { get => _maxArmour; }
}
