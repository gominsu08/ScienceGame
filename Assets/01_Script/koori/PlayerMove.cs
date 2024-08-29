using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float _nowPower = 1;
    public float NowPower
    {
        get { return _nowPower; }
        set
        {
            if (value > 0f)
                _nowPower = value;
            else
            {
                _nowPower = 0.1f;
            }
        }
    }

    [SerializeField] private float _decelerationRate = 0.5f; // 감속 비율
    [SerializeField] private LineRenderer _lineRenderer; // 라인 렌더러
    private Vector3 _startPos;
    private Vector3 _dir;
    private Rigidbody2D _rb;
    private bool _isMoving = false; // 움직이는 중인지 확인하는 변수
    private float _chcker;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _lineRenderer = _lineRenderer.GetComponent<LineRenderer>(); // 라인 렌더러 가져오기
        _lineRenderer.positionCount = 2; // 라인 렌더러의 점 개수 설정
        _lineRenderer.enabled = false; // 초기에는 라인 렌더러 비활성화
    }

    private void Update()
    {
        if (!_isMoving) // 움직이는 중이 아닐 때만 입력 체크
        {
            CheckClick();
        }
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            // 속도가 0보다 크면 감속
            if (_rb.velocity.magnitude > 3)
            {
                _rb.velocity = Vector2.Lerp(_rb.velocity, Vector2.zero, _decelerationRate * Time.deltaTime);
            }
            else
            {
                // 속도가 0에 가까워지면 멈춤
                _rb.velocity = Vector2.zero;
                _isMoving = false;
            }
        }
    }

    private void CheckClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 드래그 시작 지점 저장
            _startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _startPos.z = 0f; // Z축 값 고정
            _lineRenderer.enabled = true; // 라인 렌더러 활성화
        }
        else if (Input.GetMouseButton(0))
        {
            DragCount();
        }
        else if (Input.GetMouseButtonUp(0)) // 마우스 버튼을 땠을 때 Shot() 호출
        {
            if (!(NowPower <= 0))
            {
                Shot();
            }
            _lineRenderer.enabled = false; // 라인 렌더러 비활성화
        }
    }

    private void DragCount()
    {
        Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentPos.z = 0f; // Z축 값 고정

        NowPower = (currentPos - _startPos).magnitude;
        _dir = -(currentPos - _startPos).normalized; // 방향 벡터 계산

        // 예상 이동 거리 계산
        float predictedDistance = CalculatePredictedDistance(NowPower);

        // 라인 렌더러 업데이트
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, transform.position + _dir * predictedDistance);
    }

    private void Shot()
    {
        _rb.AddForce(_dir * NowPower, ForceMode2D.Impulse);
        NowPower = 0; // 파워 초기화
        _isMoving = true; // 움직임 시작
    }

    // 예상 이동 거리 계산 함수
    private float CalculatePredictedDistance(float power)
    {
        // 여기에 실제 물리 계산을 적용하여 예상 이동 거리를 계산합니다.
        // 현재는 단순히 power 값을 그대로 반환합니다.
        // 실제 게임에서는 질량, 마찰, 중력 등을 고려하여 계산해야 합니다.
        return power;
    }

    private void RingCheck()
    {
        Collider2D nowRing =  Physics2D.OverlapCircle(transform.position, _chcker);
    }
}