using UnityEngine;

public class TowerSlot : MonoBehaviour
{
    [SerializeField] private GameObject[] towerPrefabs;
    private ResourceManager resourceManager;
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
            TowerSelectMenuManager.Instance.HideSlotTowerMenu();
            Destroy(this.gameObject);
        }
        
    }
}
