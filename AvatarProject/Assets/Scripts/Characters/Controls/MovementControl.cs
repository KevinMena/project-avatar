using AvatarBA.Managers;
using System.Collections;
using UnityEngine;

namespace AvatarBA
{
    public class MovementControl : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField]
        protected float m_rotationSpeed = 0;

        [SerializeField]
        protected GameObject m_aim;

        protected Core m_core;
        protected CharacterController _controller;
        protected Rigidbody m_rigidbody;

        protected Vector3 m_movementDirection;
        protected Vector3 m_aimDirection;
        protected float m_speed;
        protected bool m_canMove = true;

        private bool m_inMovement = false;
        private bool m_resetDrag = false;
        private float m_drag = 0;

        protected readonly int MoveAnimation = Animator.StringToHash("Run");
        protected const string MOVEMENT_SPEED_STAT = "movementSpeed";

        public void DisableMovement() 
        {
            m_canMove = false;
            m_rigidbody.velocity = Vector3.zero;
            m_rigidbody.angularVelocity = Vector3.zero;
        }
        
        public void EnableMovement() => m_canMove = true;

        protected void Awake()
        {
            m_core = GetComponent<Core>();
            m_rigidbody = GetComponent<Rigidbody>();
            m_drag =  m_rigidbody.drag;
        }

        protected virtual void Update()
        {
            RotateAim();
            ResetDrag();
        }

        protected virtual void FixedUpdate()
        {
            if (m_canMove)
            {
                Move();
                Rotate();
            }
        }

        /// <summary>
        /// Updates the current movement state
        /// </summary>
        public void UpdateState(InputState newState) 
        {
            m_movementDirection = newState.MovementDirection;
            m_aimDirection = newState.AimDirection;
            m_speed = newState.Speed;
        }

        /// <summary>
        /// Calculates and moves the player.
        /// </summary>
        protected void Move()
        {
            // If not velocity just not move at all
            if (m_inMovement && m_movementDirection == Vector3.zero)
            {
                m_core.Animation.PlayInitialAnimation(MoveAnimation);
                m_inMovement = false;
                return;
            }

            if (m_movementDirection == Vector3.zero)
                return;

            //Set animation
            m_core.Animation.PlayAnimation(MoveAnimation);

            // Cache the current movement speed
            if (m_speed == -1)
                m_speed = m_core.Stats.GetStat(MOVEMENT_SPEED_STAT);

            // Cache velocity last frame
            Vector3 previousVelocity = m_rigidbody.velocity;

            // Apply speed and calculate desire position
            Vector3 desiredVelocity = m_movementDirection * m_speed;
            Vector3 velocityChange = desiredVelocity - previousVelocity;
            m_rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
            m_inMovement = true;
        }

        /// <summary>
        /// Calculates and rotates the player
        /// </summary>
        protected void Rotate()
        {
            if (m_movementDirection == Vector3.zero)
                return;

            // Calculate and apply new rotation
            Quaternion targetRotation = Quaternion.LookRotation(m_movementDirection);
            Quaternion desiredRotation = Quaternion.Slerp(m_rigidbody.rotation, targetRotation, m_rotationSpeed * Time.deltaTime);
            m_rigidbody.MoveRotation(desiredRotation);
        }

        protected void RotateAim()
        {
            if (m_aimDirection == Vector3.zero)
                return;

            // Calculate and apply new rotation
            Quaternion targetRotation = Quaternion.LookRotation(m_aimDirection);
            Quaternion desiredRotation = Quaternion.Slerp(m_aim.transform.rotation, targetRotation, m_rotationSpeed * Time.deltaTime);
            m_aim.transform.rotation = desiredRotation;
        }

        public void LoseControl(float loseTime)
        {
            StartCoroutine(LoseControlCO(loseTime));
        }

        protected IEnumerator LoseControlCO(float loseTime)
        {
            float finishedTime = Time.time + loseTime;

            DisableMovement();

            while (Time.time < finishedTime)
            {
                // Lose control effects
                yield return null;
            }

            EnableMovement();
        }

        private void ResetDrag()
        {
            if(m_resetDrag)
            {
                m_resetDrag = false;
                m_rigidbody.drag = m_drag;
            }
        }

        public void Impulse(Vector3 direction, float speed)
        {
            m_rigidbody.drag = 0;

            Vector3 desiredVelocity = direction * speed ;
            m_rigidbody.AddForce(desiredVelocity, ForceMode.Impulse);

            m_resetDrag = true;
        }
    }
}