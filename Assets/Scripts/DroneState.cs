namespace Assets.Scripts
{
    internal abstract class DroneState
    {
        protected Drone Drone;
        public abstract void Update();

        internal void SetDrone(Drone drone)
        {
            Drone = drone;
        }
    }
}