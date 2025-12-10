using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;


public class FlyingTextManager : MonoBehaviour
{
    public static FlyingTextManager Instance;
    public GameObject flyingTextPrefab;
    public GameObject flyingScrapPrefab;
    public float flyDistance = 2f;
    public float speed = 1f;

    private void Awake()
    {
        Instance = this; 
    }

    public void SpawnText(Vector3 spawnPoint, string text, UnityEngine.Color color, float scale)
    {
        // --- Offset vers le haut ---
        float verticalOffset = 1f; // ajuste selon ton jeu
        spawnPoint += Vector3.up * verticalOffset;

        // --- Random autour de l'ennemi ---
        float radius = 0.5f; // rayon autour de l’ennemi
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * radius;
        spawnPoint += new Vector3(randomCircle.x, 0, randomCircle.y);

        GameObject spawnedText = Instantiate(flyingTextPrefab);

        if (spawnedText != null)
        {
            spawnedText.transform.position = spawnPoint;

            spawnedText.transform.GetChild(0).GetChild(0)
                .GetComponent<FlyingText>()
                .SetupText(text, color, scale);

            StartCoroutine(Move(spawnedText));
        }
    }

    public void SpawnScrap(Vector3 spawnPoint, ItemSO item)
    {
        if (item == null)
        {
            Debug.LogWarning("⚠ SpawnScrap appelé avec un item NULL !");
            return;
        }

        if (flyingScrapPrefab == null)
        {
            Debug.LogError("❌ flyingScrapPrefab n’est pas assigné dans le FlyingTextManager !");
            return;
        }

        // --- Offset vers le haut ---
        float verticalOffset = 1f;
        spawnPoint += Vector3.up * verticalOffset;

        // --- Random autour ---
        float radius = 3.5f;
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * radius;
        spawnPoint += new Vector3(randomCircle.x, 0, randomCircle.y);

        GameObject spawnedObj = Instantiate(flyingScrapPrefab);
        if (spawnedObj == null)
            return;

        spawnedObj.transform.position = spawnPoint;

        // ✔ sécurisation de la hiérarchie
        var scrapComponent = spawnedObj.GetComponentInChildren<FlyingScrap>();
        if (scrapComponent == null)
        {
            Debug.LogError("❌ Aucun FlyingScrap trouvé dans le prefab !");
            return;
        }

        scrapComponent.SetUpScrap(item);
        StartCoroutine(Move(spawnedObj));
    }


    public IEnumerator Move(GameObject spawnedText) 
    {
        float targetY = spawnedText.transform.position.y + flyDistance;
        while(spawnedText.transform.position.y < targetY) 
        {
            spawnedText.transform.position += Vector3.up * speed*Time.deltaTime;
            yield return null;
        }
        Destroy(spawnedText);
    }
    

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
