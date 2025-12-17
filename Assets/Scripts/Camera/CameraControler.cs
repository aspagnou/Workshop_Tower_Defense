using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float fastMultiplier = 2f;

    [Header("Map bounds (XZ)")]
    public Vector2 minBounds = new Vector2(-20f, -20f);
    public Vector2 maxBounds = new Vector2(20f, 20f);

    [Header("Zoom")]
    public float zoomSpeed = 6f;
    public float zoomSmoothness = 5f;
    public float minZoom = 5f;
    public float maxZoom = 22f;

    private float _currentZoom;

    private Camera _camera;
    private Camera _UICamera;

    private bool isSpeeded = false;

    // 🟢 raccourci propre
    private float dt => Time.unscaledDeltaTime;

    void Start()
    {
        _camera = Camera.main;
        _UICamera = transform.GetChild(0).GetComponent<Camera>();

        _currentZoom = _camera.fieldOfView;
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
        ClampPosition();

        if (Input.GetKeyDown(KeyCode.Y))
        {
            ToggleTimeScale();
        }
    }

    // ----------------------------------------------
    // 🔵 Déplacement caméra (INSENSIBLE au slow-mo)
    // ----------------------------------------------
    void HandleMovement()
    {
        float speed = Input.GetKey(KeyCode.LeftShift)
            ? moveSpeed * fastMultiplier
            : moveSpeed;

        Vector3 dir = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) dir += transform.forward;
        if (Input.GetKey(KeyCode.S)) dir -= transform.forward;
        if (Input.GetKey(KeyCode.D)) dir += transform.right;
        if (Input.GetKey(KeyCode.A)) dir -= transform.right;

        dir.y = 0f;

        if (dir.sqrMagnitude > 0.001f)
        {
            transform.position += dir.normalized * speed * dt;
        }
    }

    // ----------------------------------------------
    // 🟡 Zoom fluide molette (INSENSIBLE au slow-mo)
    // ----------------------------------------------
    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.0001f)
        {
            _currentZoom -= scroll * zoomSpeed;
            _currentZoom = Mathf.Clamp(_currentZoom, minZoom, maxZoom);
        }

        _camera.fieldOfView = Mathf.Lerp(
            _camera.fieldOfView,
            _currentZoom,
            dt * zoomSmoothness
        );

        _UICamera.fieldOfView = Mathf.Lerp(
            _UICamera.fieldOfView,
            _currentZoom,
            dt * zoomSmoothness
        );
    }

    // ----------------------------------------------
    // 🔴 Clamp map
    // ----------------------------------------------
    void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.z = Mathf.Clamp(pos.z, minBounds.y, maxBounds.y);
        transform.position = pos;
    }

    // ----------------------------------------------
    // ⏱️ Toggle time scale (DEBUG)
    // ----------------------------------------------
    void ToggleTimeScale()
    {
        isSpeeded = !isSpeeded;
        Time.timeScale = isSpeeded ? 3.5f : 1f;
    }
}
