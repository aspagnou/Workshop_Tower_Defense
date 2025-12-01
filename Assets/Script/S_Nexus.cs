using UnityEngine;

public class S_Nexus : MonoBehaviour
{

    [SerializeField] public float pvNexus = 1000f;
    public float _pvNexus => pvNexus;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pvNexus <= 0)
        {
            Debug.Log("Le nexus à été détruit");
            Destroy(gameObject);
        }
    }

    
}
