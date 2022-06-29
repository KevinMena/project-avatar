using UnityEngine;

namespace AvatarBA.Combat
{
    public class CombatBaseState : CombatState
    {
        protected string stateName;

        // Animator for now
        protected Animator animator;

        protected float attackDuration;

        protected bool continueCombo = false;

        protected float timer;

        public CombatBaseState(CombatHandler owner) : base(owner) { }

        public override void OnEnter()
        {
            animator = owner.GetComponent<Animator>();
            timer = 0;
        }

        public override void OnExit()
        {
            continueCombo = false;
        }

        public override void OnUpdate()
        {
            timer += Time.deltaTime;
        }

        public void ContinueCombo()
        {
            continueCombo = true;
        }
    }
}