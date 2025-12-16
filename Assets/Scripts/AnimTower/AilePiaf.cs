using UnityEngine;

public class AilePiaf : MonoBehaviour
{
    [Header("Wings")]
    [SerializeField] private Transform leftWing;
    [SerializeField] private Transform rightWing;

    [Header("Flap Settings")]
    [SerializeField] private float flapAngle = 30f;   // Amplitude du battement
    [SerializeField] private float flapSpeed = 6f;    // Vitesse

    private Quaternion leftInitialRot;
    private Quaternion rightInitialRot;

    void Start()
    {
        if (leftWing != null)
            leftInitialRot = leftWing.localRotation;

        if (rightWing != null)
            rightInitialRot = rightWing.localRotation;
    }

    void Update()
    {
        float flap = Mathf.Sin(Time.time * flapSpeed) * flapAngle;

        if (leftWing != null)
            leftWing.localRotation = leftInitialRot * Quaternion.Euler(0f, 0f, flap);

        if (rightWing != null)
            rightWing.localRotation = rightInitialRot * Quaternion.Euler( 0f, 0f,-flap);
    }
}
