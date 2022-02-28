namespace Assets.Scripts
{
    using UnityEngine;

    internal abstract class DroneState
    {
        protected Drone Drone;

        public abstract Vector3 GetTargetDirection();
        public abstract float GetSpeedModifier();

        public abstract void Update();

        internal void SetDrone(Drone drone)
        {
            Drone = drone;
        }
    }
}