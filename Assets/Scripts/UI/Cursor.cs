using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField] private GameObject _greenState = null;
    [SerializeField] private GameObject _redState = null;


    private void Update()
    {
        Vector3 cursorPos = Input.mousePosition;
        transform.position = cursorPos;
    }

    public void SetGreenState()
    {
        _greenState.SetActive(true);
        _redState.SetActive(false);
    }

    public void SetRedState()
    {
        _greenState.SetActive(false);
        _redState.SetActive(true);
    }
}
