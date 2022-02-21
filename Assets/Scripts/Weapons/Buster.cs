namespace Assets.Scripts.Weapons
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class Buster : MonoBehaviour
    {
        [SerializeField] private InputActionReference _triggerAction;

        private BusterState _busterState;

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
            Debug.Log("Pew pew!");
            _busterState = BusterState.Idle;
        }


        private void TriggerStart(InputAction.CallbackContext obj)
        {
            _busterState = BusterState.Charging;
        }

        // Update is called once per frame
        private void Update()
        {
            if (_busterState == BusterState.Charging)
            {
            }
        }

        private void OnDestroy()
        {
            _triggerAction.action.started -= TriggerStart;
        }
    }
}