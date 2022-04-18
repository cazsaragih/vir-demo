using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Transform cube;

    public enum Mode
    {
        Freeview,
        Viewpoint
    }

    [SerializeField, Range(2, 4)] private int zoomMultiplier;

    private Camera cam;
    private float deltaX;
    private float deltaY;
    private bool isDragging;
    private float normalFOV;
    [SerializeField] private float velocity;
   
    public Mode mode;

    // Start is called before the first frame update
    void Awake()
    {
        cam = GetComponent<Camera>();

        LeanTouch.OnFingerUpdate += LeanTouch_OnFingerUpdate;
    }

    private void Start()
    {
        normalFOV = cam.fieldOfView;
    }

    private void LeanTouch_OnFingerUpdate(LeanFinger finger)
    {
        isDragging = true;
        deltaX = finger.ScreenDelta.x;
        deltaY = finger.ScreenDelta.y;
    }

    private void Update()
    {
        switch (mode)
        {
            case Mode.Freeview:
                Freeview();
                break;
            case Mode.Viewpoint:
                Viewpoint();
                break;
        }
    }

    private void Freeview()
    {
        if (Input.GetMouseButton(1))
        {
            float zoomFOV = normalFOV / zoomMultiplier;
            if (!Mathf.Approximately(cam.fieldOfView, zoomFOV))
            {
                cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, zoomFOV, ref velocity, 0.2f);
            }
        }
        else 
        {
            if (!Mathf.Approximately(cam.fieldOfView, normalFOV))
            {
                cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, normalFOV, ref velocity, 0.2f);
            }
        }


        if (!isDragging)
            return;

        transform.position = cube.position;
        transform.Rotate(Vector3.up, deltaX * 20 * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.right, -deltaY * 20 * Time.deltaTime);
        transform.Translate(0, 0, -10);

        isDragging = false;
    }

    private void Viewpoint()
    {
        
    }

    private void SetState(Mode mode)
    {
        this.mode = mode;
    }

    private void OnDestroy()
    {
        LeanTouch.OnFingerUpdate += LeanTouch_OnFingerUpdate;
    }
}
