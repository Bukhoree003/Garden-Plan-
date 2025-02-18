using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int index;
    [SerializeField] Sprite sprite;
    [SerializeField] Image image;
    [SerializeField] GameObject prefab;
    Button onClick;
    [SerializeField] Vector3 offset;
    BuildManager buildManager;
    BuildingManager buildM;

    private void Start()
    {
        if (sprite != null && image !=null)
            image.sprite = sprite;
        onClick = GetComponent<Button>();
        onClick.onClick.AddListener(OnClickB);
        buildManager = FindAnyObjectByType<BuildManager>();
        buildM = FindAnyObjectByType<BuildingManager>();
    }

    private void OnClickB()
    {
        if (buildManager != null)
        {
            buildManager.itemIndex = index;
            buildManager.UpdateUI();
        }
        if (buildM != null)
        {
            buildM.itemIndex = index;
            buildM.Clicked();
            if(prefab != null)
            Instantiate(prefab, offset, Quaternion.identity).transform.SetParent(buildM.container);
        }
    }
}
