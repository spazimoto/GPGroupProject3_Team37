using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera camera;
    private float targetFOV;
    private float fov;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        targetFOV = camera.fieldOfView;
        fov = targetFOV;
    }

    private void Update()
    {
        float fovSpeed = 4f;
        fov = Mathf.Lerp(fov, targetFOV, Time.deltaTime * fovSpeed);
        camera.fieldOfView = fov;
    }

    public void SetCameraFOV (float targetFOV)
    {
        this.targetFOV = targetFOV;
    }
    
}