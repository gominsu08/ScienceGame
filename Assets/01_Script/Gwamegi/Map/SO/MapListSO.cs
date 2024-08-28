using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/MapData/MapList")]
public class MapListSO : ScriptableObject
{
    public List<MapSO> mapDatas = new List<MapSO>();
}
