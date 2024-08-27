using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSinglton<GameManager>
{

    [SerializeField] private RectTransform _escPanel;
    [SerializeField] private float _startPosition, _time;
    public GameObject player;
    private bool _isESC, _isMove = true;



    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ESC();
        }
    }

    public void ESC()
    {
        if(!_isMove) return;

        if (!_isESC)
        {
            _isMove = false;
            _escPanel.DOAnchorPos(Vector2.zero, _time).OnComplete(() => _isMove = true);
        }
        else
        {
            _isMove = false;
            _escPanel.DOAnchorPos(new Vector2(0, _startPosition), _time).OnComplete( () => _isMove = true);
        }

        _isESC = !_isESC;
    }


}
