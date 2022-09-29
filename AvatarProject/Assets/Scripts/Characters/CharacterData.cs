using UnityEngine;

using AvatarBA.Stats;

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

        [SerializeField]
        private StatBase[] _stats;

        public int Id => _id;

        public string Name => _name;

        public string Description => _description;

        public Sprite Icon => _icon;

        public GameObject Prefab => _prefab;

        public ref StatBase[] Stats => ref _stats;
    }
}
