using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControl : MonoBehaviour
{
    [SerializeField] private string scene;
    Button b;
    // Start is called before the first frame update
    void Start()
    {
        if (b = GetComponent<Button>())
        {
            b.onClick.AddListener( () => OpenScene(scene));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
}
