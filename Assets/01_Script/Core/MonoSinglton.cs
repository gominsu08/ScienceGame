using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSinglton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    private static bool _isDestroy = false;

    public static T Instance
    {

    get {
            if(_isDestroy == true)
                _instance = null;
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                    Debug.LogError("해당 오브젝트가 존재하지 않습니다");
                else 
                    _isDestroy = false;
            }
            return _instance; 
        }
    }

    private void OnDisable()
    {
        _isDestroy = true;
    }

}
