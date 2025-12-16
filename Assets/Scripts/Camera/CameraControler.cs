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
    public float zoomSpeed = 6f;          // vitesse du zoom scroll
    public float zoomSmoothness = 5f;     // fluidité
    public float minZoom = 5f;            // FOV min → zoom IN
    public float maxZoom = 22f;           // FOV max → zoom OUT
    private float _currentZoom;

    private Camera _camera;
    private Camera _UIcamera;

    void Start()
    {
        _camera = Camera.main;
        _UIcamera = transform.GetChild(0).GetComponent<Camera>();
        _currentZoom = _camera.fieldOfView;  // initialise le zoom à la valeur actuelle
        
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
        ClampPosition();
    }

    // ----------------------------------------------
    // 🔵 Déplacement iso basé sur la direction caméra
    // ----------------------------------------------
    void HandleMovement()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * fastMultiplier : moveSpeed;

        Vector3 dir = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) dir += transform.forward;
        if (Input.GetKey(KeyCode.S)) dir -= transform.forward;
        if (Input.GetKey(KeyCode.D)) dir += transform.right;
        if (Input.GetKey(KeyCode.A)) dir -= transform.right;

        dir.y = 0; // pas de montée/descente
        transform.position += dir.normalized * speed * Time.deltaTime;
    }

    // ----------------------------------------------
    // 🟡 Zoom fluide à la molette (FOV)
    // ----------------------------------------------
    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            _currentZoom -= scroll * zoomSpeed;                  // scroll ↑ = zoom IN
            _currentZoom = Mathf.Clamp(_currentZoom, minZoom, maxZoom);
        }

        // zoom fluide
        _camera.fieldOfView = Mathf.Lerp(
            _camera.fieldOfView,
            _currentZoom,
            Time.deltaTime * zoomSmoothness
        );
        _UIcamera.fieldOfView = Mathf.Lerp(
            _camera.fieldOfView,
            _currentZoom,
            Time.deltaTime * zoomSmoothness
        );
    }

    // ----------------------------------------------
    // 🔴 Limites de la map
    // ----------------------------------------------
    void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.z = Mathf.Clamp(pos.z, minBounds.y, maxBounds.y);
        transform.position = pos;
    }
}
