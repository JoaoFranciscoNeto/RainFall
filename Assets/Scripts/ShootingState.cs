namespace Assets.Scripts
{
    using UnityEngine;

    internal class ShootingState : DroneState
    {
        public override void Update()
        {
            if (!Drone.IsTargetVisible())
            {
                Drone.TransitionTo(new ChasingState());
            }

            var vectorToTarget = Drone.Target.transform.position-Drone.transform.position;
            Drone.RotateTo(vectorToTarget);

            var dist = vectorToTarget.magnitude;

            var targetDist = Mathf.Clamp(dist, Drone.EffectiveRange.Item1, Drone.EffectiveRange.Item2);
            Drone.GetComponent<Rigidbody>()
                .AddForce(Drone.Speed * Time.deltaTime * (Drone.transform.forward * (dist-targetDist)).normalized);
        }
    }
}