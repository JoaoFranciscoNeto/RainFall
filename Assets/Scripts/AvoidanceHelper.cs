namespace Assets.Scripts
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public static class AvoidanceHelper
    {
        private const int NCircles = 3;
        private const float ConeAngle = 60;
        private const int PointsPerCircle = 6;

        private static Vector3[] directions;

        public static Vector3[] RayCastDirections => directions ??= SpherePoints().ToArray();

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

        private static IEnumerable<Vector3> SpherePoints()
        {
            for (var i = 0; i < 50; i++)
            {
                yield return Random.onUnitSphere;
            }
        }
    }
}