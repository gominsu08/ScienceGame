using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private MapManager manager;

    private bool isCreate;

    [SerializeField] private float _speed;

    private void Start()
    {
        manager = FindObjectOfType<MapManager>();
    }

    private void Update()
    {
        if (transform.position.y <=10 && !isCreate)
        {
            manager.MapCreate();
            isCreate = true;
        }

        if (transform.position.y <= manager.endPosition.position.y)
        {
            Destroy(gameObject);
            return;
        }

        Move();
    }


    public void Move()
    {
        transform.position += Vector3.down * _speed * Time.deltaTime;
    }
}
