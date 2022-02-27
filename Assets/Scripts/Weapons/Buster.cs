namespace Assets.Scripts.Weapons
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class Buster : MonoBehaviour
    {
        public GameObject ProjectileGameObject;

        public GameObject Muzzle;
        public float ProjectileSpeed;

        public float ChargeFactor;
        public float MaxChargeTime;

        private readonly Color _baseColor = Color.yellow;
        private readonly Color _chargedColor = Color.red;


        [SerializeField] private InputActionReference _triggerAction;

        private BusterState _busterState;

        private float _chargeTime;
        private Renderer _renderer;

        private enum BusterState
        {
            Idle,
            Charging,
            Charged,
        }

        private void Awake()
        {
            _triggerAction.action.started += TriggerStart;
            _triggerAction.action.canceled += TriggerCancelled;
            _renderer = GetComponentInChildren<Renderer>();
        }

        private void TriggerCancelled(InputAction.CallbackContext obj)
        {
            _busterState = BusterState.Idle;

            var projectile = Instantiate(ProjectileGameObject, Muzzle.transform.position, Quaternion.identity);

            var projectileSpeed = ProjectileSpeed+_chargeTime*ChargeFactor;

            projectile.GetComponent<Rigidbody>().velocity = transform.forward*projectileSpeed;
            _chargeTime = 0;
        }


        private void TriggerStart(InputAction.CallbackContext obj)
        {
            _busterState = BusterState.Charging;
        }

        private void Update()
        {
            if (_busterState == BusterState.Charging)
            {
                _chargeTime += Time.deltaTime;

                if (_chargeTime >= MaxChargeTime)
                {
                    _chargeTime = MaxChargeTime;
                    _busterState = BusterState.Charged;
                }
            }

            _renderer.material.color = Color.Lerp(_baseColor, _chargedColor, _chargeTime/MaxChargeTime);
        }

        private void OnDestroy()
        {
            _triggerAction.action.started -= TriggerStart;
            _triggerAction.action.canceled -= TriggerCancelled;
        }
    }
}