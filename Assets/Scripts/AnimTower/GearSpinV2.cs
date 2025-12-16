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

    void Update()
    {
        RotateArray(rotateX, Vector3.right);
        RotateArray(rotateY, Vector3.up);
        RotateArray(rotateZ, Vector3.forward);
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
}


