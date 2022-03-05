namespace Assets.Scripts
{
    using UnityEngine;

    internal class ChasingState : DroneState
    {
        private const int Dist = 1;

        private readonly Vector3 _strafeDir;

        private readonly Vector3 _boxVolume = new (10, 5, 10);

        public ChasingState()
        {
            _strafeDir = Random.onUnitSphere;
            _strafeDir.Scale(_boxVolume);
            Debug.Log(_strafeDir);
        }


        public override void Update()
        {
            if (Drone.IsTargetVisible())
            {
                Drone.TransitionTo(new ShootingState());
            }

            var t = Drone.Target.transform.position-Drone.transform.position;
            var a = Avoidance();

            Drone.GetComponent<Rigidbody>().AddForce(
                Drone.Speed * Time.deltaTime * (t+a).normalized);

            Debug.DrawLine(Drone.transform.position, _strafeDir, Color.green);
        }


        private Vector3 Avoidance()
        {
            var weightedDirection = Vector3.zero;

            foreach (var direction in AvoidanceHelper.RayCastDirections)
            {
                var td = Drone.transform.TransformDirection(direction);

                if (!Physics.Raycast(Drone.transform.position, td, Dist))
                {
                    continue;
                }

                weightedDirection -= td;
                Debug.DrawLine(Drone.transform.position, Drone.transform.position+td, Color.red);
            }

            return weightedDirection;
        }
    }
}