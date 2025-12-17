using UnityEngine;

public class AiguilleCraft : MonoBehaviour
{
    [Header("Target rotations en degrés")]
    public float angle1 = 0f;
    public float angle2 = 180f;

    [Header("Vitesse de rotation")]
    public float rotationSpeed = 500f;

    private float targetAngle;
    private bool isAtPos1 = true;
    private float dt => Time.unscaledDeltaTime;
    private void Start()
    {
        // On part sur la première position
        targetAngle = angle1;
        SetRotationInstant(angle1);
    }
    
    private void Update()
    {
        // Rotation vers l’angle cible
        float currentZ = transform.eulerAngles.z;
        float newAngle = Mathf.MoveTowardsAngle(currentZ, targetAngle, rotationSpeed * dt);

        transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }

    /// <summary>
    /// Force la rotation directement sur l'angle donné (pas d'animation)
    /// </summary>
    private void SetRotationInstant(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    /// <summary>
    /// Appelle cette fonction pour changer la position de l’aiguille.
    /// </summary>
    public void TogglePosition()
    {
        isAtPos1 = !isAtPos1;

        targetAngle = isAtPos1 ? angle1 : angle2;
    }
}
