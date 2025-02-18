using UnityEngine;
//using UnityEngine.InputSystem.EnhancedTouch;

public class TestTouch : MonoBehaviour
{
    [Header("Scale")]
    [SerializeField] Camera cc;
    public GameObject target;
    private void Update()
    {
        // ตรวจจับการสัมผัสบนหน้าจอ
        if (Input.touchCount == 1)
        {
            // สัมผัสทั้งสองนิ้ว
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = cc.ScreenPointToRay(touch.position);
                RaycastHit[] hits;
                // ยิง Raycast และเก็บข้อมูลการชน
                hits = Physics.RaycastAll(ray);
                if (hits.Length > 0)
                {
                    foreach (RaycastHit hit in hits)
                    {
                        Debug.Log(hit.transform.name);
                        //text.text = $"{hit.transform.name}";
                    }
                }
            }
        }
    }
}
