using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] Transform cam;
    //[SerializeField] Transform firepoint;
    [SerializeField] float sensitivity;
    float headRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xRotate = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float yRotate = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime * -1f;
        
        headRotation += yRotate;
        headRotation = Mathf.Clamp(headRotation, -90.0f, 90.0f);
        cam.localRotation = Quaternion.Euler(headRotation, 0f, 0f);
        //firepoint.localRotation = Quaternion.Euler(headRotation, 0f, 0f);
        transform.Rotate(Vector3.up * xRotate);
        //cam.localEulerAngles = new Vector3(headRotation, 0f, 0f);
    }
}
