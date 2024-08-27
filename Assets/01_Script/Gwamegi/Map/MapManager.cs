using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public MapListSO mapListSO;

    public Transform startPosition, endPosition, createPosition;


    private void Awake()
    {
        MapCreate();
    }

    public void MapCreate()
    {
        Instantiate(mapListSO.mapDatas[Random.Range(0, mapListSO.mapDatas.Count)].map, startPosition.position,Quaternion.identity);
    }
}
