using UnityEngine;

public class Clicker : MonoBehaviour
{
    public Camera cam;           // Laisse vide  prend automatiquement Camera.main
    public LayerMask towerLayer; // Met "Tower" dans l’inspecteur

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
                tower.inventoryMemory.DisplayInventory();


            }
        }
        else
        {
            // Clic dans le vide  fermer
            //DeselectTower();
        }
    }

    private void SelectTower(BaseTower tower)
    {
        // Si on clique la même tour  toggle
        if (tower == currentSelectedTower)
        {
            DeselectTower();
            return;
        }

        // Fermer l’ancienne
        if (currentSelectedTower != null)
            currentSelectedTower.HideGrid();

        // Ouvrir la nouvelle
        currentSelectedTower = tower;
        currentSelectedTower.ShowGrid();
    }

    private void DeselectTower()
    {
        if (currentSelectedTower != null)
        {
            currentSelectedTower.HideGrid();
            currentSelectedTower = null;
        }
    }
}


