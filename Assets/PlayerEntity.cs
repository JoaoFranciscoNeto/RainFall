namespace Assets
{
    using Assets.Scripts;
    using UnityEngine;

    public class PlayerEntity : Entity
    {
        private CharacterController _characterController;

        // Start is called before the first frame update
        private void Start()
        {
            _characterController = GetComponentInParent<CharacterController>();
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}