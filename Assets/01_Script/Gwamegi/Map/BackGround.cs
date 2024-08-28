using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{

    [SerializeField] private Transform _startposition, _endPosition;

    [SerializeField] private float _moveSpeed;

    private void Update()
    {
        transform.position += Vector3.down * _moveSpeed * Time.deltaTime;

        if(transform.position.y <= _endPosition.position.y)
        {
            transform.position = _startposition.position;
        }
    }

    
}
