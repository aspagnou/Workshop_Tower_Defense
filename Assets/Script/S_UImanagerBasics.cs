using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;


public class S_UImanagerBasics : MonoBehaviour
{
    [SerializeField] private S_WaveManager waveManager; 

    public S_Wave[] waves;



    [Header ("Text UI")]
    [SerializeField] private TextMeshProUGUI waveCounterLabel;
    [SerializeField] private TextMeshProUGUI firstWaveLabel;
    [SerializeField] private TextMeshProUGUI nextWaveLabel;

    //[SerializeField] private float countDown = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
      
        
    }
}
