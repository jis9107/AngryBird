using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToScreen : MonoBehaviour
{
    public GameObject circle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("아무것도 하지 않고 찍은 것 : " + Input.mousePosition);
            Debug.Log("WorldToScreenPoint : " + Camera.main.WorldToScreenPoint(Input.mousePosition));
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Debug.Log("ScreenToWorld : " + mousePos);
            Instantiate(circle, mousePos, Quaternion.identity);
        }
    }
}
