using TMPro;
using UnityEngine;
using UnityEngine.VFX;

public class TowerSlot : MonoBehaviour
{
    [SerializeField] private GameObject[] towerPrefabs;
    private TMP_Text[] costsText;
    private ResourceManager resourceManager;
    public VisualEffectAsset spawnVFX;  // Le VFX à jouer lors du spawn
    public Vector3 vfxOffset = Vector3.up * 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resourceManager = FindAnyObjectByType<ResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnTower(int i) 
    {
        if (resourceManager.mana < towerPrefabs[i].GetComponent<BaseTower>().spawnCost)
        {
            Debug.Log("Pas assez de mana pour construire cette tour !");
            return;
        }
        else 
        {
            resourceManager.SpendMana(towerPrefabs[i].GetComponent<BaseTower>().spawnCost);
            Instantiate(towerPrefabs[i], transform.position, Quaternion.identity);
            if (spawnVFX != null)
            {
                VisualEffect vfx = new GameObject("SpawnVFX").AddComponent<VisualEffect>();
                vfx.visualEffectAsset = spawnVFX;

                vfx.transform.position = transform.position + vfxOffset;

                vfx.Play();

                Destroy(vfx.gameObject, 2f); // Auto-clean
            }
            TowerSelectMenuManager.Instance.HideSlotTowerMenu();
            Destroy(this.gameObject);
        }
        
    }
    public void UpdateCostText() 
    {
        
        costsText = TowerSelectMenuManager.Instance.slotCostTexts;
        for (int i = 0; i < towerPrefabs.Length; i++)
        {
            costsText[i].text = towerPrefabs[i].GetComponent<BaseTower>().spawnCost.ToString();
        }
        
    }
   
    

    
}
