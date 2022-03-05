namespace Assets.Scripts
{
    using UnityEngine;

    public class ConstantSpawner : MonoBehaviour
    {
        public float Interval;
        public GameObject Prefab;
        public Bounds Bounds;
        private float _timer;


        private void Awake()
        {
            _timer = Interval;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                _timer = Interval;

                var pos = new Vector3(
                    Random.Range(-Bounds.extents.x, Bounds.extents.x),
                    Random.Range(-Bounds.extents.y, Bounds.extents.y),
                    Random.Range(-Bounds.extents.z, Bounds.extents.z));

                pos = transform.TransformPoint(pos);
                var obj = Instantiate(Prefab, pos, Quaternion.identity);

                obj.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
                obj.GetComponent<FloatyThing>().MovementTarget = Camera.main.transform;
                obj.GetComponent<FloatyThing>().RotationTarget = Camera.main.transform;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position, Bounds.size);
        }
    }
}