using System.Collections.Generic;
using System.Linq;
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

        Debug.Log($"Transitioned to {state.GetType()}");
    }

    private void Awake()
    {
        TransitionTo(new ChasingState());
    }

    private void Update()
    {
        var dir = _state.GetTargetDirection();

        if (dir != Vector3.zero)
        {
            RotateTo(dir);
        }

        transform.position += Speed * Time.deltaTime * transform.forward;
        /*
        var dirToTarget = Target.transform.position-transform.position;

        if (Physics.Raycast(transform.position, dirToTarget, out var hitTarget))
        {
            if (hitTarget.transform.IsChildOf(Target.transform))
            {
                RotateTo(dirToTarget);
            }
            else
            {
                var direction = WeightedDirection();

                var goalRot = Quaternion.LookRotation(direction == Vector3.zero ? dirToTarget : direction);

                transform.rotation = Quaternion.Lerp(transform.rotation, goalRot, Time.deltaTime * RotationSpeed);

                transform.position += Speed * Time.deltaTime * transform.forward;
            }
        }*/
    }

    private void RotateTo(Vector3 direction)
    {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * RotationSpeed);
    }
}

internal abstract class DroneState
{
    protected Drone Drone;

    public abstract Vector3 GetTargetDirection();
    public abstract float GetSpeedModifier();

    internal void SetDrone(Drone drone)
    {
        Drone = drone;
    }
}

internal class ChasingState : DroneState
{
    private const int NCircles = 3;
    private const float ConeAngle = 30;
    private const int PointsPerCircle = 6;
    private const int Dist = 10;

    private Vector3[] _rayCastDirections;

    public ChasingState()
    {
        _rayCastDirections = ConePoints().ToArray();
    }

    public override Vector3 GetTargetDirection()
    {
        if (IsAimingAtTarget())
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

    private static IEnumerable<Vector3> ConePoints()
    {
        const float radiusIncrement = Mathf.Deg2Rad * ConeAngle / NCircles;
        const float angleIncrement = 2 * Mathf.PI / PointsPerCircle;

        for (var c = 0; c < NCircles; c++)
        {
            var radius = radiusIncrement * (c+1);

            for (var i = 0; i < PointsPerCircle; i++)
            {
                var p = new Vector2(Mathf.Sin(i * angleIncrement), Mathf.Cos(i * angleIncrement)).normalized * Mathf.Sin(radius);
                yield return new Vector3(p.x, p.y, Mathf.Cos(radius));
            }
        }
    }

    private bool CanSeeTarget()
    {
        var dirToTarget = Drone.Target.transform.position-Drone.transform.position;

        return Physics.Raycast(Drone.transform.position, dirToTarget, out var hitTarget) &&
               hitTarget.transform.IsChildOf(Drone.Target.transform);
    }

    private bool IsAimingAtTarget() => Physics.Raycast(Drone.transform.position, Drone.transform.forward, out var hitTarget) &&
                                       hitTarget.transform.IsChildOf(Drone.Target.transform);

    private Vector3 WeightedDirection()
    {
        var weightedDirection = Drone.Target.transform.position-Drone.transform.position;

        foreach (var direction in _rayCastDirections)
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

        return weightedDirection;
    }

    private void InitiateObstacleAvoidance()
    {
        _rayCastDirections = ConePoints().ToArray();
    }
}

internal class ShootingState : DroneState
{
    public override Vector3 GetTargetDirection() => Vector3.forward;
    public override float GetSpeedModifier() => 0;
}