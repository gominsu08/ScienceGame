using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _nowPower;
    private Vector3 _startPos;
    private Vector3 _dir;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckClick();
    }
    private void CheckClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // �巡�� ���� ���� ����
            _startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _startPos.z = 0f; // Z�� �� ����
        }
        else if (Input.GetMouseButton(0))
        {
            DragCount();

        }
        else
        {
            if (!(_nowPower <= 0))
            {
                Shot();
            }
        }
    }

    private void DragCount()
    {
        Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentPos.z = 0f; // Z�� �� ����

        _nowPower = (currentPos - _startPos).magnitude;
        _dir = -(currentPos - _startPos).normalized; // ���� ���� ���
    }

    private void Shot()
    {
        _rb.AddForce(_dir*_nowPower*Time.deltaTime, ForceMode2D.Impulse);
    }
}