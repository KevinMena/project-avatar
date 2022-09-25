using System.Collections;
using UnityEngine;

namespace AvatarBA.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Effect_Dash", menuName = "Abilities/Effects/Dash Effect")]
    public class SpawnProjectileEffect : AbilityEffect
    {
        [SerializeField]
        GameObject _projectile;

        public override IEnumerator Cast(GameObject owner)
        {
            Instantiate(_projectile, owner.transform.position, Quaternion.identity);
            yield return null;
        }
    }
}