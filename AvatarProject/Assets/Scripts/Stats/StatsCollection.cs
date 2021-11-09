using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.Stats
{
    [CreateAssetMenu(fileName = "StatsCollection_New", menuName ="Stats/Collection")]
    public class StatsCollection : ScriptableObject
    {
        [SerializeField] private List<StatType> _stats = new List<StatType>();

        public List<StatType> Stats
        {
            get => _stats;
        }
    }
}
