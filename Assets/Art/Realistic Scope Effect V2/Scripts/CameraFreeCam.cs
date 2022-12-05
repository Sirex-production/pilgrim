using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFreeCam : MonoBehaviour
{

    public float Speed = 5;

    void Update()
    {
        if (!Input.GetKey(KeyCode.LeftAlt))
            transform.Translate(Input.GetAxisRaw("Horizontal") * Speed * Time.deltaTime, Input.GetAxisRaw("Vertical") * Speed * Time.deltaTime, Input.GetAxisRaw("Mouse ScrollWheel") * Speed * 100 * Time.deltaTime);
    }
}
