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

    [SerializeField] private float _decelerationRate = 0.5f; // ���� ����
    [SerializeField] private LineRenderer _lineRenderer; // ���� ������
    private Vector3 _startPos;
    private Vector3 _dir;
    private Rigidbody2D _rb;
    private bool _isMoving = false; // �����̴� ������ Ȯ���ϴ� ����
    private float _chcker;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _lineRenderer = _lineRenderer.GetComponent<LineRenderer>(); // ���� ������ ��������
        _lineRenderer.positionCount = 2; // ���� �������� �� ���� ����
        _lineRenderer.enabled = false; // �ʱ⿡�� ���� ������ ��Ȱ��ȭ
    }

    private void Update()
    {
        if (!_isMoving) // �����̴� ���� �ƴ� ���� �Է� üũ
        {
            CheckClick();
        }
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            // �ӵ��� 0���� ũ�� ����
            if (_rb.velocity.magnitude > 3)
            {
                _rb.velocity = Vector2.Lerp(_rb.velocity, Vector2.zero, _decelerationRate * Time.deltaTime);
            }
            else
            {
                // �ӵ��� 0�� ��������� ����
                _rb.velocity = Vector2.zero;
                _isMoving = false;
            }
        }
    }

    private void CheckClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // �巡�� ���� ���� ����
            _startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _startPos.z = 0f; // Z�� �� ����
            _lineRenderer.enabled = true; // ���� ������ Ȱ��ȭ
        }
        else if (Input.GetMouseButton(0))
        {
            DragCount();
        }
        else if (Input.GetMouseButtonUp(0)) // ���콺 ��ư�� ���� �� Shot() ȣ��
        {
            if (!(NowPower <= 0))
            {
                Shot();
            }
            _lineRenderer.enabled = false; // ���� ������ ��Ȱ��ȭ
        }
    }

    private void DragCount()
    {
        Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentPos.z = 0f; // Z�� �� ����

        NowPower = (currentPos - _startPos).magnitude;
        _dir = -(currentPos - _startPos).normalized; // ���� ���� ���

        // ���� �̵� �Ÿ� ���
        float predictedDistance = CalculatePredictedDistance(NowPower);

        // ���� ������ ������Ʈ
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, transform.position + _dir * predictedDistance);
    }

    private void Shot()
    {
        _rb.AddForce(_dir * NowPower, ForceMode2D.Impulse);
        NowPower = 0; // �Ŀ� �ʱ�ȭ
        _isMoving = true; // ������ ����
    }

    // ���� �̵� �Ÿ� ��� �Լ�
    private float CalculatePredictedDistance(float power)
    {
        // ���⿡ ���� ���� ����� �����Ͽ� ���� �̵� �Ÿ��� ����մϴ�.
        // ����� �ܼ��� power ���� �״�� ��ȯ�մϴ�.
        // ���� ���ӿ����� ����, ����, �߷� ���� ����Ͽ� ����ؾ� �մϴ�.
        return power;
    }

    private void RingCheck()
    {
        Collider2D nowRing =  Physics2D.OverlapCircle(transform.position, _chcker);
    }
}