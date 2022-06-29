using UnityEngine;

namespace AvatarBA.Combat
{
    public class CombatBaseState : CombatState
    {
        protected string stateName;
        protected CombatState nextState;

        // Animator for now
        protected Animator animator;
        protected string animationName;

        protected float attackDuration = 0;
        protected float attackRange = 0;
        protected float damage = 0;

        protected bool continueCombo = false;

        protected float timer = 0;
        private float _attackTriggeredTimer = 0;

        public CombatBaseState(CombatHandler owner) : base(owner) 
        {
            animator = owner.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            timer = 0;
            damage = CalculateDamage();
            attackDuration = CalculateAttackDuration();
            animator.SetTrigger(animationName);
        }

        public override void OnExit()
        {
            continueCombo = false;
        }

        public override void OnUpdate()
        {
            timer += Time.deltaTime;
            _attackTriggeredTimer -= Time.deltaTime;
            
            if(_attackTriggeredTimer > 0)
                continueCombo = true;

            if(timer > attackDuration)
            {
                if(continueCombo && nextState != null)
                    owner.SetState(nextState);
                else
                    owner.SetStateToInitial();
            }
        }

        public void TriggerInput()
        {
            _attackTriggeredTimer = 0.5f;
        }

        private float CalculateDamage()
        {
            return 0;
        }

        private float CalculateAttackDuration()
        {
            AnimationClip[] animations = animator.runtimeAnimatorController.animationClips;
            foreach (var animation in animations)
            {
                if(animation.name == animationName)
                    return animation.length;
            }

            return 0;
        }
    }
}