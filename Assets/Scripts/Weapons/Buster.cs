namespace Assets.Scripts.Weapons
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class Buster : MonoBehaviour
    {
        public GameObject ProjectileGameObject;
        [SerializeField] private InputActionReference _triggerAction;

        private BusterState _busterState;

        private float _chargeTime;

        private enum BusterState
        {
            Idle,
            Charging,
        }

        private void Awake()
        {
            _triggerAction.action.started += TriggerStart;
            _triggerAction.action.canceled += TriggerCancelled;
        }

        private void TriggerCancelled(InputAction.CallbackContext obj)
        {
            _busterState = BusterState.Idle;

            var projectile = Instantiate(ProjectileGameObject, transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().velocity = transform.forward*_chargeTime;
        }


        private void TriggerStart(InputAction.CallbackContext obj)
        {
            _busterState = BusterState.Charging;
            _chargeTime = 0;
        }

        private void Update()
        {
            if (_busterState == BusterState.Charging)
            {
                _chargeTime += Time.deltaTime;
            }
        }

        private void OnDestroy()
        {
            _triggerAction.action.started -= TriggerStart;
        }
    }
}