using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestLeanManager : MonoBehaviour
{
    LeanPinchScale pinchScale;
    LeanTwistRotateAxis rotateAxis;
    [SerializeField] GameObject f;
    [SerializeField] TextMeshProUGUI touchCountText;
    // Start is called before the first frame update
    void Start()
    {
        pinchScale = f.GetComponent<LeanPinchScale>();
        rotateAxis = f.GetComponent<LeanTwistRotateAxis>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
        {
            pinchScale.enabled = false;
            rotateAxis.enabled = false;
            touchCountText.text = $"No Touch Count : {Input.touchCount}";
        }
        else
        {
            pinchScale.enabled = true;
            rotateAxis.enabled = true;
            touchCountText.text = $" Touch Count : {Input.touchCount}";
        }
    }
}
