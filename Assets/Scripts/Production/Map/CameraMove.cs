using UnityEngine;

public interface IResetCamera
{
    void ResetCameraPosition();
}

public interface IMoveCamera
{
    void MoveCameraToOrigin(Vector3 origin);
}

public interface ICamera : IMoveCamera, IResetCamera { }

public class CameraMove : MonoBehaviour, ICamera
{
    private Vector3 m_InitialPosition;

    private void Awake()
    {
        m_InitialPosition = transform.position;
    }

    public void MoveCameraToOrigin(Vector3 origin)
    {
        transform.position += origin;
    }

    public void ResetCameraPosition()
    {
        transform.position = m_InitialPosition;
    }     
}
