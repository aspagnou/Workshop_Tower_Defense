using UnityEngine;

public class GearSpinV2 : MonoBehaviour
{
    [Header("Rotation Speed")]
    [SerializeField] private float rotationSpeed = 100f;

    [Header("Rotate on X axis")]
    [SerializeField] private Transform[] rotateX;

    [Header("Rotate on Y axis")]
    [SerializeField] private Transform[] rotateY;

    [Header("Rotate on Z axis")]
    [SerializeField] private Transform[] rotateZ;

    [Header("Floating")]
    public float floatSpeed = 2f;
    public float floatAmplitude = 0.3f;
    private Vector3 startPosition;

    [SerializeField] private Transform body;

    private void Start()
    {
        startPosition = body.transform.position;
    }
    void Update()
    {
        RotateArray(rotateX, Vector3.right);
        RotateArray(rotateY, Vector3.up);
        RotateArray(rotateZ, Vector3.forward);
        FloatMovement();
    }

    void RotateArray(Transform[] gears, Vector3 axis)
    {
        if (gears == null) return;

        for (int i = 0; i < gears.Length; i++)
        {
            if (gears[i] == null) continue;

            gears[i].Rotate(axis, rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
    void FloatMovement()
    {
        float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        body.position = startPosition + Vector3.up * offsetY;
    }

}


