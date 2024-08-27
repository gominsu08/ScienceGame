using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class InteractionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _endScale = 1.5f, time = 0.5f, _startScale = 1f;

    [SerializeField] private RectTransform _trm;

    [SerializeField] private string _sceneName;

    [SerializeField] private bool _isShow;


    public void EnterUI()
    {
        _trm.DOScale(_endScale, time);
    }

    public void ExitUI()
    {
        _trm.DOScale(_startScale, time);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isShow)
            EnterUI();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isShow)
            ExitUI();
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }

    
}
