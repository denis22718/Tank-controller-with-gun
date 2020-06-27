using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    [SerializeField] private GameObject _explosionParticles = null;

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(_explosionParticles, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
