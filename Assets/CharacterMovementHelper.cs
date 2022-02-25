using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CharacterMovementHelper : MonoBehaviour
{
    private XRRig _xrRig;
    private CharacterController _characterController;
    private CharacterControllerDriver _characterControllerDriver;

    protected virtual void UpdateCharacterController()
    {
        if (_xrRig == null || _characterController == null)
        {
            return;
        }

        var height = Mathf.Clamp(_xrRig.cameraInRigSpaceHeight, _characterControllerDriver.minHeight, _characterControllerDriver.maxHeight);

        var center = _xrRig.cameraInRigSpacePos;
        center.y = height/2f+_characterController.skinWidth;

        _characterController.height = height;
        _characterController.center = center;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _xrRig = GetComponent<XRRig>();
        _characterController = GetComponent<CharacterController>();
        _characterControllerDriver = GetComponent<CharacterControllerDriver>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateCharacterController();
    }
}