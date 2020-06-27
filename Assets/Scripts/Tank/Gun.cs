using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject _gun = null;
    [SerializeField] private GameObject _tower = null;

    [Header("Rotation bounds"), Space(10)]
    [SerializeField] private Vector2 _towerBounds = new Vector2(-40,40);
    [SerializeField] private Vector2 _gunBounds = new Vector2(0,20);

    [Header("Shooting"), Space(10)]
    [SerializeField] private GameObject _shell = null;
    [SerializeField] private Transform _shootingPoint = null;
    [SerializeField] private GameObject _shotExplosion = null;

    private Vector3 _towerEulerAngles;

    private Cursor _cursor;

    private void Start()
    {
        _cursor = FindObjectOfType<Cursor>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shot();
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (!hit.collider.CompareTag("Wall"))
            {
                // rotate tower
                var lookPos = hit.point - _tower.transform.position;
                var rotation = Quaternion.LookRotation(lookPos);
                // rotation boundaries
                rotation.y = Mathf.Clamp(rotation.y, _towerBounds.x, _towerBounds.y);
                rotation.z = 0;
                rotation.x = 0;
                _tower.transform.rotation = Quaternion.Slerp(_tower.transform.rotation, rotation, Time.deltaTime * 1.25f);
                _tower.transform.rotation.eulerAngles.Set(0, rotation.y, 0);
                _towerEulerAngles = _tower.transform.rotation.eulerAngles;

                // rotate gun
                lookPos = hit.point - _gun.transform.position;
                rotation = Quaternion.LookRotation(lookPos);
                // rotation boundaries
                rotation.x = Mathf.Clamp(rotation.x, _gunBounds.x, _gunBounds.y);
                _gun.transform.rotation = Quaternion.Slerp(_gun.transform.rotation, rotation, Time.deltaTime * 3);
                _gun.transform.rotation.eulerAngles.Set(rotation.x, 0, 0);

                // The cursor must not go outside the world
                _cursor.SetGreenState();
            } else
            {
                _cursor.SetRedState();
            }
        }
    }

    private void Shot()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (!hit.collider.CompareTag("Wall"))
            {
                // spawn new shell
                Rigidbody newShell = Instantiate(_shell, _shootingPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
                // spawn particle explosion
                Instantiate(_shotExplosion, _shootingPoint.position, Quaternion.identity);

                // calculate force to the target
                Vector3 force = calcBallisticVelocityVector(_shootingPoint.position, hit.point, 15);
                newShell.AddForce(force, ForceMode.Impulse);
            }
        }
    }

    private Vector3 calcBallisticVelocityVector(Vector3 source, Vector3 target, float angle)
    {
        Vector3 direction = target - source;
        float h = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;
        float a = angle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(a);
        distance += h / Mathf.Tan(a);

        // calculate velocity
        float velocity = 1;
        if (Vector3.Distance(source, target) > 10)
        {
            velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        }
        return velocity * direction.normalized;
    }

    public bool CanRotationTank()
    {
        return _towerEulerAngles.y <= _towerBounds.x || _towerEulerAngles.y >= _towerBounds.y;
    }
}
