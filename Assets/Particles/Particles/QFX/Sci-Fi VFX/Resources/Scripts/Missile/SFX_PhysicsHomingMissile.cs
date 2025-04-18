using UnityEngine;

// ReSharper disable once CheckNamespace
namespace QFX.SFX
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SFX_DistanceCollisionDetector))]
    public class SFX_PhysicsHomingMissile : SFX_ControlledObject
    {
        public float Velocity;
        public float LaunchForce;
        public float Turn;

        public bool FollowTarget;
        public Transform TargetTransform;
        public Vector3 TargetPosition;

        public Vector3 LaunchPosition;
        public float FocusOnTargetDelay;

        public bool IsDebug;

        private Rigidbody _rigidbody;
        private bool _isCollided;
        private bool _shouldFocusOnTarget;

        private SFX_DistanceCollisionDetector m_sfxDistanceCollisionDetector;

        private bool _wasCollided;

        public override void Run()
        {
            base.Run();

            transform.LookAt(LaunchPosition);
            _rigidbody.AddForce((LaunchPosition - transform.position).normalized * LaunchForce, ForceMode.Impulse);

            SFX_InvokeUtil.RunLater(this, () => { _shouldFocusOnTarget = true; }, FocusOnTargetDelay);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            m_sfxDistanceCollisionDetector = GetComponent<SFX_DistanceCollisionDetector>();

            var collisionProviders = GetComponents<SFX_ICollisionsProvider>();
            foreach (var collisionsProvider in collisionProviders)
            {
                collisionsProvider.OnCollision += delegate
                {
                    if (_wasCollided)
                        return;

                    Destroy(gameObject);

                    _wasCollided = true;
                };
            }
        }

        private void FixedUpdate()
        {
            if (!IsRunning)
                return;

            if (!_shouldFocusOnTarget)
                return;

            Vector3 targetPosition;
            if (FollowTarget)
                targetPosition = TargetTransform != null
                    ? TargetTransform.transform.position
                    : transform.position + transform.forward;
            else
                targetPosition = TargetPosition;

            m_sfxDistanceCollisionDetector.TargetPosition = targetPosition;

            var direction = (targetPosition - transform.position).normalized;
            _rigidbody.linearVelocity = transform.forward * Velocity;
            var targetRotation = Quaternion.LookRotation(direction);
            _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, Turn));
        }

        private void OnDrawGizmos()
        {
            if (!IsDebug)
                return;

            Gizmos.DrawSphere(LaunchPosition, 0.2f);
            if (TargetTransform != null)
                Gizmos.DrawSphere(TargetTransform.transform.position, 0.2f);
        }
    }
}