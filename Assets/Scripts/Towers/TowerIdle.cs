using UnityEngine;

public class TowerIdle : MonoBehaviour
{
    [Header("Helice")]
    [SerializeField] private Transform[] propeller;
    [SerializeField] private float propellerSpeed = 600f;

    [Header("Floating Movement")]
    [SerializeField] private Transform body;
    [SerializeField] private float floatAmplitude = 0.3f;
    [SerializeField] private float floatSpeed = 2f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        RotatePropeller();
        FloatMovement();
    }

    // ---------------- ROTATION HELICE ----------------
    void RotatePropeller()
    {
        if (propeller == null) return;

        for (int i = 0; i < propeller.Length; i++)
            propeller[i].Rotate(Vector3.up, propellerSpeed * Time.deltaTime, Space.Self);
    }

    // ---------------- FLOTTEMENT ----------------
    void FloatMovement()
    {
        float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        body.position = startPosition + Vector3.up * offsetY;
    }
}
