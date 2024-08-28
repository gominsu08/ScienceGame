using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private float _currentScore;
    [SerializeField] private float _addScore;

    [SerializeField] private TMP_Text _text;

    private void Update()
    {
        _currentScore += _addScore;

        _text.text = $"Score : {Mathf.Floor(_currentScore * 100f) / 100f}";
    }


}
