using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 0.1f;
    [SerializeField, Range(0, 1f)] private float _rotationSpeedTank = 0.1f;
    
    private CharacterController _tank;
    private Gun _gun;
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
        _tank = GetComponent<CharacterController>();
        _gun = GetComponent<Gun>();
    }

    private void FixedUpdate()
    {
        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // calculate move tank
        Vector3 move = (v * _cam.transform.forward + h * _cam.transform.right) * _moveSpeed + Physics.gravity;
        _tank.Move(move);

        // smooth rotation tank
        if (_gun.CanRotationTank())
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-_cam.transform.right), _rotationSpeedTank);
        }
    }
}
