using UnityEngine;

public class Clicker : MonoBehaviour
{
    public Camera cam;           // Laisse vide  prend automatiquement Camera.main
    public LayerMask towerLayer; // Met "Tower" dans l’inspecteur
    [SerializeField] private UI_Manager ui_Manager;
    private BaseTower currentSelectedTower = null;
    

    private void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleClick();
    }

    private void HandleClick()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // On touche une tour ?
        if (Physics.Raycast(ray, out hit,999f, towerLayer))
        {
            Debug.Log("Clicked on tower: " + hit.collider.gameObject.name);
            BaseTower tower = hit.collider.GetComponent<BaseTower>();
           
            if (tower != null)
            {  
                SelectTower(tower);
                tower.OnTowerSelected();
            }
        }
        else
        {
            // Clic dans le vide  fermer
            //DeselectTower();
        }
    }

    public void SelectTower(BaseTower tower)
    {
        // Si on clique la même tour, toggle
        if (tower == currentSelectedTower)
        {
            DeselectTower();
            return;
        }

        // Fermer l’ancienne
        if (currentSelectedTower != null)
        {
            currentSelectedTower.OnTowerDeselected();
        }

        // Ouvrir la nouvelle
        currentSelectedTower = tower;
        currentSelectedTower.OnTowerSelected();
    }

    public void DeselectTower()
    {
        Debug.Log("Je ferme");
        if (currentSelectedTower != null)
        {
            currentSelectedTower.OnTowerDeselected();
            currentSelectedTower = null;
        }
    }

    public void OpenTowerInventoryGrid() 
    {
        if (currentSelectedTower != null) 
        { 
            currentSelectedTower.ShowGrid();
            currentSelectedTower.inventoryMemory.DisplayInventory();
            ui_Manager.ShowGearMenu();
        }
    }
}


