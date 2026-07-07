using UnityEngine;
using UnityEngine.InputSystem;

public class Custom_cursor : MonoBehaviour
{
    void Awake()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        transform.position = mousePosition;    
    }


    void Update()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        transform.position = mousePosition;
    }
}
