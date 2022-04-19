using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace VirTest.Camera
{
    public class FreeviewCamera : MonoBehaviour, ICamera
    {
        public UnityEngine.Camera Camera { get; set; }

        public string ModeName { get; private set; }

        public float DeltaX { get; private set; }

        public float DeltaY { get; private set; }

        public bool IsDragging { get; private set; }

        [SerializeField] private List<GameObject> objectsToOrbit;
        [SerializeField, Range(2, 4)] private int zoomMultiplier = 2;
        [SerializeField] private float orbitSpeed = 20f;
        [SerializeField] private float orbitOffsetZ = -10f;

        private GameObject currentObjToOrbit;
        private float normalFOV;
        private float velocity;

        void Awake()
        {
            Camera = GetComponent<UnityEngine.Camera>();
            ModeName = "Freeview";
        }

        public void Activate(Vector3 prevPosition = default, Quaternion prevRotation = default)
        {
            gameObject.SetActive(true);
            normalFOV = Camera.fieldOfView;
            transform.SetPositionAndRotation(prevPosition, prevRotation);
            Switch();
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            currentObjToOrbit = null;
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

            Orbit(currentObjToOrbit);

            IsDragging = false;
        }

        public void Switch()
        {
            if (currentObjToOrbit == null)
            {
                currentObjToOrbit = objectsToOrbit[0];
                SetInitialOrbitPos(currentObjToOrbit);
            }
            else
            {
                int index = objectsToOrbit.IndexOf(currentObjToOrbit);
                if (index < 0)
                    return;

                index = (index + 1) % objectsToOrbit.Count;
                currentObjToOrbit = objectsToOrbit[index];
                SetInitialOrbitPos(currentObjToOrbit);
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

        private void Orbit(GameObject obj)
        {
            if (obj == null)
                return;

            transform.position = obj.transform.position;
            transform.Rotate(Vector3.up, DeltaX * orbitSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.right, -DeltaY * orbitSpeed * Time.deltaTime);
            transform.Translate(0, 0, orbitOffsetZ);
        }

        private void SetInitialOrbitPos(GameObject go)
        {
            if (go == null)
                return;

            transform.DOMove(new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z + orbitOffsetZ), 1f)
                .SetEase(Ease.OutQuint)
                .OnUpdate(() => { transform.LookAt(go.transform); });
        }
    }
}