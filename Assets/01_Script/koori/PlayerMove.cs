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
    [SerializeField] private GameObject _cam;
    [SerializeField] private float _smoothSpeed = 5f; // ī�޶� �̵� �ε巯�� ����
    private Camera _camCompo;
    private Vector3 _startPos;
    private Vector3 _dir;
    private Rigidbody2D _rb;
    private bool _isMoving = false; // �����̴� ������ Ȯ���ϴ� ����

    private void Awake()
    {
        _camCompo = _cam.GetComponent<Camera>();
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

    private void LateUpdate()
    {
        if (_isMoving)
        {
            _camCompo.orthographicSize = Mathf.Lerp(_camCompo.orthographicSize, 2.5f, Time.deltaTime * _smoothSpeed); // �ε巯�� ��
            Vector3 vecTarget = new Vector3(transform.position.x, transform.position.y, -10);
            _cam.transform.position = Vector3.Lerp(_cam.transform.position, vecTarget, Time.deltaTime * _smoothSpeed); // �ε巯�� �̵�
        }
        else
        {
            // �÷��̾� ��ġ�� �������� ī�޶� ��ġ ����
            Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, -10);
            _cam.transform.position = Vector3.Lerp(_cam.transform.position, targetPosition, Time.deltaTime * _smoothSpeed);

            // �÷��̾� ��ġ�� �������� ī�޶� �� ����
            float targetSize = 5 / NowPower;
            _camCompo.orthographicSize = Mathf.Lerp(_camCompo.orthographicSize, targetSize, Time.deltaTime * _smoothSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            // �ӵ��� 0���� ũ�� ����
            if (_rb.velocity.magnitude > 1)
            {
                _rb.velocity = Vector2.Lerp(_rb.velocity, Vector2.zero, _decelerationRate * Time.deltaTime);
            }
            else
            {
                // �ӵ��� 0�� ��������� ����
                _rb.velocity = Vector2.zero;
                _isMoving = false;

                // ī�޶� ��ġ �� �� �ʱ�ȭ
                _cam.transform.position = new Vector3(0, 0, -10);
                _camCompo.orthographicSize = 5f;
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

        //ī�޶� �� (�ε巴��)
        _camCompo.orthographicSize = Mathf.Lerp(_camCompo.orthographicSize, (5 / NowPower), Time.deltaTime * _smoothSpeed);
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
}