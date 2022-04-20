using UnityEngine;

namespace VirTest.Camera
{
    public interface ICamera
    {
        UnityEngine.Camera Camera { get; }

        string ModeName { get; }

        float DeltaX { get; }

        float DeltaY { get; }

        bool IsDragging { get; }

        void OnUpdate();

        void ZoomIn();

        void ZoomOut();

        void Activate(Vector3 prevPosition = default, Quaternion prevRotation = default);

        void Deactivate();

        void Switch();

        void OnFingerUpdate(float dX, float dY);
    }
}