namespace Assets.Scripts.Weapons
{
    using UnityEngine;

    public class Projectile : MonoBehaviour
    {
        public float Damage;

        [SerializeField]
        private readonly float _timeToLive = 5;

        private float _timeAlive;

        private void Awake()
        {
            _timeAlive = 0;
        }

        // Update is called once per frame
        private void Update()
        {
            _timeAlive += Time.deltaTime;

            if (_timeAlive >= _timeToLive)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            var entity = collision.gameObject.GetComponentInParent<Entity>();

            if (entity != null)
            {
                entity.ApplyDamage(Damage);
            }


            /*
            entity.GetComponent<Rigidbody>().AddExplosionForce(10, collision.contacts.First().point, 5);


            var colliders = Physics.OverlapSphere(collision.contacts.First().point, 10);

            foreach (var hit in colliders)
            {
                var rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(100, collision.contacts.First().point, 10, 3.0F);
                }
            }*/

            //Destroy(gameObject);
        }
    }
}