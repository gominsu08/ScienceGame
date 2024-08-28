using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private GameObject _player;

    [SerializeField] private LayerMask _playerLayer;

    private void Start()
    {
        _player = GameManager.Instance.player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.layer);
        Debug.Log(_playerLayer);

        if (collision.gameObject.layer == 22)
        {
            collision.gameObject.transform.DOMove(transform.position,1);
        }
    }
}
