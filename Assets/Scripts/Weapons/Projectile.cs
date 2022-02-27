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

            entity.ApplyDamage(Damage);

            Destroy(gameObject);
        }
    }
}