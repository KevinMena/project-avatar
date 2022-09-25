using UnityEngine;

namespace AvatarBA
{
    [CreateAssetMenu(fileName = "Character_", menuName ="Characters/New Character")]
    public class CharacterData : ScriptableObject
    {
        [SerializeField] 
        private int _id = 0;

        [SerializeField] 
        private string _name = "New Character";

        [TextArea]
        [SerializeField] 
        private string _description = "Description";

        [SerializeField] 
        private Sprite _icon = null;

        [SerializeField] 
        private GameObject _prefab = null;

        [SerializeField, Range(0, 200)]
        private float _baseHealth = 0;

        [SerializeField, Range(0, 10)]
        private float _baseAttackPower = 0;

        [SerializeField, Range(0, 1)]
        private float _baseAttackSpeed = 0;

        [SerializeField, Range(0, 10)]
        private float _baseDefense = 0;

        [SerializeField, Range(0, 10)]
        private float _baseMovementSpeed = 0;

        [SerializeField, Range(0, 10)]
        private float _baseSpiritPower = 0;

        public int Id => _id;

        public string Name => _name;

        public string Description => _description;

        public Sprite Icon => _icon;

        public GameObject Prefab => _prefab;

        public float BaseHealth => _baseHealth;
        public float BaseAttackPower => _baseAttackPower;
        public float BaseAttackSpeed => _baseAttackSpeed;
        public float BaseDefense => _baseDefense;
        public float BaseMovementSpeed => _baseMovementSpeed;
        public float BaseSpiritPower => _baseSpiritPower;
    }
}
