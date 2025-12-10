using TMPro;
using UnityEngine;

public class FlyingText : MonoBehaviour
{
    public TMP_Text flyingText;
    public Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }
    public void SetupText(string text, Color color, float scale)
    {
        flyingText.text = text;
        flyingText.color = color;
        transform.localScale = Vector3.one * scale;
    }





    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cam.transform);
        transform.rotation = Quaternion.LookRotation(cam.transform.forward);
    }
}
