using System;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }

    void Update()
    {
        transform.LookAt(camera.transform.position, -Vector3.up);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
    }
}