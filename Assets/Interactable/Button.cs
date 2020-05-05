using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent enterAction;
    public UnityEvent exitAction;

    Color defaultColor;
    Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = transform.Find("PressPlate").GetComponent<Renderer>();
        defaultColor = renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        enterAction.Invoke();
        renderer.material.color = Color.red;
    }

    private void OnTriggerStay(Collider other)
    {
    }

    private void OnTriggerExit(Collider other)
    {
        exitAction.Invoke();
        renderer.material.color = defaultColor;
    }
}
