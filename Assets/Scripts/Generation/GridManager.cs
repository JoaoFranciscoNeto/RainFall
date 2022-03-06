namespace Assets.Scripts.Generation
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class GridManager : MonoBehaviour
    {
        public Vector2Int Size = new (3, 3);
        public GameObject CellPrefab;
        public GameObject PlayerPrefab;
        private IEnumerable<Vector3> _gridPoints;

        private void Awake()
        {
            var cellBounds = CellPrefab.GetComponent<Renderer>().localBounds;
            _gridPoints = CreateGridPoints(Size.x, Size.y, cellBounds).ToArray();


            foreach (var gridPoint in _gridPoints)
            {
                Instantiate(
                    CellPrefab,
                    gridPoint,
                    Quaternion.identity,
                    transform);
            }

            Instantiate(PlayerPrefab, transform.position, Quaternion.identity);
        }

        private void OnDrawGizmosSelected()
        {
            if (_gridPoints == null)
            {
                return;
            }

            foreach (var gridPoint in _gridPoints)
            {
                Gizmos.DrawSphere(gridPoint, .2f);
            }
        }


        private IEnumerable<Vector3> CreateGridPoints(int width, int height, Bounds cellBounds)
        {
            var totalSize = cellBounds.size;
            totalSize.Scale(CellPrefab.transform.localScale);

            totalSize.Scale(new Vector3(Size.x, 0, Size.y));
            var offset = new Vector3(height-1, 0, width-1) / 2;

            for (var x = 0; x < height; x++)
            {
                for (var y = 0; y < width; y++)
                {
                    /*
                                        var pos = new Vector3(
                                            x * cellBounds.size.x * CellPrefab.transform.localScale.x,
                                            0 * cellBounds.size.y * CellPrefab.transform.localScale.y,
                                            y * cellBounds.size.z * CellPrefab.transform.localScale.z);
                                        pos -= totalSize / 2;*/
                    var pos = new Vector3(x, 0, y)-offset;
                    yield return new Vector3(
                        pos.x * cellBounds.size.x * CellPrefab.transform.localScale.x,
                        0,
                        pos.z * cellBounds.size.z * CellPrefab.transform.localScale.z);
                }
            }
        }
    }
}