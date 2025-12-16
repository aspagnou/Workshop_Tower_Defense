using UnityEngine;

public class GearSpin : MonoBehaviour
{
    [System.Serializable]
    public struct GearData
    {
        public Transform gear;
        public Vector3 localAxis;   // Axe de rotation LOCAL
        public float speed;         // Vitesse (négatif = sens inverse)
    }

    [Header("Gears (1 script pour toute la tour)")]
    [SerializeField] private GearData[] gears;

    void Update()
    {
        for (int i = 0; i < gears.Length; i++)
        {
            if (gears[i].gear == null) continue;

            gears[i].gear.Rotate(
                gears[i].localAxis.normalized,
                gears[i].speed * Time.deltaTime,
                Space.Self
            );
        }
    }
}
