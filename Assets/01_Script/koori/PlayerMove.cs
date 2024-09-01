using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _maxLaunchForce = 5f;
    [SerializeField] private float _launchForceMultiplier = 10f;
    [SerializeField] private float _decelerationRate = 2f;

    [Header("Ring Settings")]
    [SerializeField] private LayerMask _ringLayer;
    [SerializeField] private float _ringCheckRadius = 0.5f;

    [Header("Visuals")]
    [SerializeField] private LineRenderer _trajectoryLineRenderer;
    [SerializeField] private GameObject _launchPositionIndicator;

    private Rigidbody2D _rigidbody;
    private Vector2 _launchStartPosition;
    private bool _isDragging = false;
    private bool _isLaunched = false;
    private Transform _currentRing;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _trajectoryLineRenderer.positionCount = 2;
        _trajectoryLineRenderer.enabled = false;

        if (_launchPositionIndicator != null)
        {
            _launchPositionIndicator.SetActive(false);
        }
    }

    private void Update()
    {
        if (!_isLaunched)
        {
            HandleInput();
            CheckForRing();
        }

        CheckScreenBounds();
    }

    private void FixedUpdate()
    {
        if (_isLaunched)
        {
            ApplyDeceleration();
            CheckForRing();
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDragging();
        }
        else if (Input.GetMouseButton(0) && _isDragging)
        {
            UpdateTrajectoryLine();
        }
        else if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            LaunchPlayer();
        }
    }

    private void StartDragging()
    {
        _isDragging = true;
        _launchStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _trajectoryLineRenderer.enabled = true;

        if (_launchPositionIndicator != null)
        {
            _launchPositionIndicator.SetActive(true);
            _launchPositionIndicator.transform.position = _launchStartPosition;
        }
    }

    private void UpdateTrajectoryLine()
    {
        Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 launchDirection = (_launchStartPosition - currentMousePosition).normalized;
        float dragDistance = Vector2.Distance(_launchStartPosition, currentMousePosition);
        float launchForce = Mathf.Clamp(dragDistance * _launchForceMultiplier, 0, _maxLaunchForce);

        _trajectoryLineRenderer.SetPosition(0, transform.position);
        _trajectoryLineRenderer.SetPosition(1, (Vector2)transform.position + launchDirection * launchForce);
    }

    private void LaunchPlayer()
    {
        _isDragging = false;
        _trajectoryLineRenderer.enabled = false;

        if (_launchPositionIndicator != null)
        {
            _launchPositionIndicator.SetActive(false);
        }

        Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 launchDirection = (_launchStartPosition - currentMousePosition).normalized;
        float dragDistance = Vector2.Distance(_launchStartPosition, currentMousePosition);
        float launchForce = Mathf.Clamp(dragDistance * _launchForceMultiplier, 0, _maxLaunchForce);

        _rigidbody.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
        _currentRing = null;

        // 여기에 추가!
        _isLaunched = true;
    }

    private void ApplyDeceleration()
    {
        _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, Vector2.zero, _decelerationRate * Time.fixedDeltaTime);

        if (_rigidbody.velocity.magnitude < 0.01f)
        {
            _rigidbody.velocity = Vector2.zero;
            _isLaunched = false;
        }
    }

    private void CheckForRing()
    {
        Collider2D ringCollider = Physics2D.OverlapCircle(transform.position, _ringCheckRadius, _ringLayer);

        if (ringCollider != null && ringCollider.transform != _currentRing)
        {
            _currentRing = ringCollider.transform;
            transform.position = _currentRing.position;
            transform.SetParent(_currentRing);

            // 수정된 부분: 조건문 추가
            if (_isLaunched && _rigidbody.velocity.magnitude < 2f) // 속도가 거의 0일 때만 _isLaunched를 false로 변경
            {
                _rigidbody.velocity = Vector2.zero;
                _isLaunched = false;
            }
        }
        else if (ringCollider == null && !_isLaunched)
        {
            Destroy(gameObject); // 링에서 벗어나면 게임 오버
        }
    }

    private void CheckScreenBounds()
    {
        Vector3 screenPosition = Camera.main.WorldToViewportPoint(transform.position);

        if (screenPosition.x < 0 || screenPosition.x > 1 || screenPosition.y < 0 || screenPosition.y > 1)
        {
            Destroy(gameObject); // 화면 밖으로 나가면 게임 오버
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _ringCheckRadius);
    }
}