using UnityEngine;
using UnityEngine.UI;

public class TowerSelectMenuManager : MonoBehaviour
{
    public static TowerSelectMenuManager Instance;

    public GameObject towerSelectPanel;
    public Camera cam;
    public GameObject mainCanvas;
    public Vector3 positionOffset = new Vector3(0, 100, 0); // Offset vertical de 100 pixels

    private BaseTower currentlySelectedTower;

    void Awake()
    {
        Instance = this;
        towerSelectPanel.SetActive(false);
    }

    public void ShowTowerSelectMenu(BaseTower tower)
    {
        currentlySelectedTower = tower;

        // Convertir la position de la tour en position écran
        Vector3 towerScreenPosition = cam.WorldToScreenPoint(tower.transform.position);

        // Ajouter l'offset
        towerScreenPosition += positionOffset;

        // Convertir la position écran en position locale dans le MainCanvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            mainCanvas.GetComponent<RectTransform>(),
            towerScreenPosition,
            null,
            out Vector2 localPosition
        );

        // Appliquer la position locale au panel
        towerSelectPanel.GetComponent<RectTransform>().localPosition = localPosition;

        // Activer le panel
        towerSelectPanel.SetActive(true);
    }

    public void HideTowerSelectMenu()
    {
        towerSelectPanel.SetActive(false);
        currentlySelectedTower = null;
    }
}
