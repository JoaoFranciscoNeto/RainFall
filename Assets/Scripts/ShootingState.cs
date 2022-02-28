namespace Assets.Scripts
{
    using UnityEngine;

    internal class ShootingState : DroneState
    {
        public override Vector3 GetTargetDirection() => Vector3.forward;
        public override float GetSpeedModifier() => 0;

        public override void Update()
        {
            if (!Drone.IsAimingAtTarget())
            {
                Drone.TransitionTo(new ChasingState());
            }
        }
    }
}