using Lean.Touch;
using System.Collections.Generic;
using UnityEngine;
using VirTest.Camera;
using VirTest.Event;

public class CameraController : MonoBehaviour
{
    [SerializeField] private List<GameObject> cameraObjects;

    private List<ICamera> cameras = new List<ICamera>();
    private ICamera currentCamera;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < cameraObjects.Count; i++)
        {
            ICamera cam = cameraObjects[i].GetComponent<ICamera>();
            if (cam != null)
                cameras.Add(cam);
        }

        EventManager.AddListener<SwitchClick>(OnSwitchClick);
        EventManager.AddListener<ModeClick>(OnModeClick);
        LeanTouch.OnFingerUpdate += LeanTouch_OnFingerUpdate;
    }

    private void Start()
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            ICamera cam = cameras[i];
            cam.Deactivate();
        }

        ChangeMode();
    }

    private void OnSwitchClick(SwitchClick e)
    {
        currentCamera.Switch();
    }

    private void OnModeClick(ModeClick e)
    {
        ChangeMode();
    }

    private void LeanTouch_OnFingerUpdate(LeanFinger finger)
    {
        if (finger.IsOverGui || finger.StartedOverGui)
            return;

        currentCamera.OnFingerUpdate(finger.ScreenDelta.x, finger.ScreenDelta.y);
    }

    private void Update()
    {
        if (currentCamera == null)
            return;

        currentCamera.OnUpdate();
    }

    private void ChangeMode()
    {
        if (currentCamera == null)
        {
            currentCamera = cameras[0];
            currentCamera.Activate();
        }
        else
        {
            int index = cameras.IndexOf(currentCamera);
            if (index < 0)
                return;

            ICamera prevCamera = currentCamera;

            index = (index + 1) % cameras.Count;
            currentCamera = cameras[index];

            Transform t = prevCamera.Camera.transform;
            prevCamera.Deactivate();
            currentCamera.Activate(t.position, t.rotation);
        }

        EventManager.TriggerEvent(new ModeChange(currentCamera.ModeName));
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<SwitchClick>(OnSwitchClick);
        EventManager.RemoveListener<ModeClick>(OnModeClick);
        LeanTouch.OnFingerUpdate -= LeanTouch_OnFingerUpdate;
    }
}
