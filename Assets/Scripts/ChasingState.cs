namespace Assets.Scripts
{
    using UnityEngine;

    internal class ChasingState : DroneState
    {
        private const int Dist = 10;

        public override Vector3 GetTargetDirection()
        {
            if (Drone.IsAimingAtTarget())
            {
                Drone.TransitionTo(new ShootingState());
                return Drone.transform.forward;
            }

            if (CanSeeTarget())
            {
                return Drone.Target.transform.position-Drone.transform.position;
            }


            return WeightedDirection();
        }

        public override float GetSpeedModifier() => 1;

        public override void Update()
        {
            var dir = GetTargetDirection();

            if (dir != Vector3.zero)
            {
                Drone.RotateTo(dir);
            }

            Drone.transform.position += Drone.Speed * Time.deltaTime * Drone.transform.forward;
        }

        private bool CanSeeTarget()
        {
            var dirToTarget = Drone.Target.transform.position-Drone.transform.position;

            return Physics.Raycast(Drone.transform.position, dirToTarget, out var hitTarget) &&
                   hitTarget.transform.IsChildOf(Drone.Target.transform);
        }


        private Vector3 WeightedDirection()
        {
            var weightedDirection = Drone.Target.transform.position-Drone.transform.position;

            foreach (var direction in AvoidanceHelper.RayCastDirections)
            {
                var td = Drone.transform.TransformDirection(direction);

                if (Physics.Raycast(Drone.transform.position, td, Dist))
                {
                    weightedDirection -= td;
                    Debug.DrawLine(Drone.transform.position, Drone.transform.position+td, Color.blue);
                }
                else
                {
                    weightedDirection += td;
                }
            }


            Debug.DrawLine(Drone.transform.position, Drone.transform.position+weightedDirection, Color.cyan);
            return weightedDirection;
        }
    }
}