namespace Assets.Scripts.Weapons
{
    using UnityEngine;

    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private const float TimeToLive = 5;

        public float Damage;

        private float _timeAlive;

        private void Awake()
        {
            _timeAlive = 0;
        }

        // Update is called once per frame
        private void Update()
        {
            _timeAlive += Time.deltaTime;

            if (_timeAlive >= TimeToLive)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject != null)
            {
                var entity = collision.gameObject.GetComponentInChildren<PlayerEntity>();

                if (entity != null)
                {
                    entity.ApplyDamage(Damage);
                    Debug.Log("Pew");
                }
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

            Destroy(gameObject);
        }
    }
}