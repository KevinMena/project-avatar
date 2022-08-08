using UnityEngine;

namespace AvatarBA.AI
{
    public abstract class Consideration : ScriptableObject
    {
        [SerializeField]
        private string _name;

        private float _score;

        public string Name => _name;
        public float Score => _score;

        public abstract float CalculateScore(GameObject owner);
    }
}