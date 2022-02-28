namespace Assets.Scripts
{
    using UnityEditor;
    using UnityEngine;

    public class Drone : Entity
    {
        public float RotationSpeed = 20f;

        public float Speed = 2f;

        [SerializeField]
        internal GameObject Target;

        private DroneState _state;

        internal void TransitionTo(DroneState state)
        {
            _state = state;
            state.SetDrone(this);
        }

        internal bool IsAimingAtTarget() => Physics.Raycast(transform.position, transform.forward, out var hitTarget) &&
                                            hitTarget.transform.IsChildOf(Target.transform);

        internal void RotateTo(Vector3 direction)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * RotationSpeed);
        }

        private void Awake()
        {
            TransitionTo(new ChasingState());
        }

        private void Update()
        {
            _state.Update();
        }

        private void OnDrawGizmos()
        {
            if (_state != null)
            {
                Handles.Label(transform.position+transform.up, _state.GetType().Name);
            }
        }
    }
}