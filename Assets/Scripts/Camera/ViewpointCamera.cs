using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirTest.Camera
{
    public class ViewpointCamera : MonoBehaviour, ICamera
    {
        public UnityEngine.Camera Camera { get; private set; }

        public string ModeName { get; private set; }

        public float DeltaX { get; private set; }

        public float DeltaY { get; private set; }

        public bool IsDragging { get; private set; }

        [SerializeField, Range(2, 4)] private int zoomMultiplier;
        [SerializeField] private List<GameObject> viewpoints;

        private GameObject currentViewpoint;
        private float normalFOV;
        private float velocity;
        
        void Awake()
        {
            Camera = GetComponent<UnityEngine.Camera>();
            ModeName = "Viewpoint";
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            Init();
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void Init()
        {
            normalFOV = Camera.fieldOfView;
            Switch();
        }

        public void OnFingerUpdate(float dX, float dY)
        {
            DeltaX = dX;
            DeltaY = dY;
            IsDragging = true;
        }

        public void OnUpdate()
        {
            if (Input.GetMouseButton(1))
            {
                ZoomIn();
            }
            else
            {
                ZoomOut();
            }

            if (!IsDragging)
                return;

            Rotate();

            IsDragging = false;
        }

        public void Switch()
        {
            if (currentViewpoint == null)
            {
                currentViewpoint = viewpoints[0];
                SetInitialPos(currentViewpoint);
            }
            else
            {
                int index = viewpoints.IndexOf(currentViewpoint);
                if (index < 0)
                    return;

                index = (index + 1) % viewpoints.Count;
                currentViewpoint = viewpoints[index];
                SetInitialPos(currentViewpoint);
            }
        }

        public void ZoomIn()
        {
            float zoomFOV = normalFOV / zoomMultiplier;
            if (!Mathf.Approximately(Camera.fieldOfView, zoomFOV))
            {
                Camera.fieldOfView = Mathf.SmoothDamp(Camera.fieldOfView, zoomFOV, ref velocity, 0.2f);
            }
        }

        public void ZoomOut()
        {
            if (!Mathf.Approximately(Camera.fieldOfView, normalFOV))
            {
                Camera.fieldOfView = Mathf.SmoothDamp(Camera.fieldOfView, normalFOV, ref velocity, 0.2f);
            }
        }
        
        private void Rotate()
        {
            transform.Rotate(new Vector3(-DeltaY, -DeltaX, 0f) * Time.deltaTime, Space.World);
        }

        private void SetInitialPos(GameObject go)
        {
            transform.position = new Vector3(go.transform.position.x,
                                             go.transform.position.y,
                                             go.transform.position.z);
        }
    }
}
