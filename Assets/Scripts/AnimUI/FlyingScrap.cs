using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class FlyingScrap : MonoBehaviour
{
    public Image flyingImage;
    public Camera cam;
    public TMP_Text text;

    private void Start()
    {
        cam = Camera.main;
        
    }
    public void SetUpScrap(ItemSO item)
    {
        
        flyingImage.sprite = item.itemIcon;
    }





    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cam.transform);
        transform.rotation = Quaternion.LookRotation(cam.transform.forward);
    }
}