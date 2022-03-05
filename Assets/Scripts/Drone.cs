namespace Assets.Scripts
{
    using System;
    using UnityEditor;
    using UnityEngine;

    public class Drone : Entity
    {
        public float RotationSpeed = 20f;

        public float Speed = 2f;

        public Tuple<float, float> EffectiveRange = new (3, 5);

        [SerializeField]
        internal GameObject Target;

        private DroneState _state;

        internal void TransitionTo(DroneState state)
        {
            _state = state;
            _state.SetDrone(this);
        }

        internal bool IsAimingAtTarget() => Physics.Raycast(transform.position, transform.forward, out var hitTarget) &&
                                            hitTarget.transform.IsChildOf(Target.transform);

        /// <summary>
        ///     Checks if the Target is obstructed from view.
        /// </summary>
        /// <returns>True if target is not obstructed.</returns>
        internal bool IsTargetVisible() => Physics.Raycast(
                                               transform.position, Target.transform.position-transform.position, out var hitTarget) &&
                                           hitTarget.transform.IsChildOf(Target.transform);

        internal void RotateTo(Vector3 direction)
        {
            gameObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.LookRotation(direction));
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