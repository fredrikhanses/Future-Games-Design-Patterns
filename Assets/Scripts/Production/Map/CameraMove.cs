using UnityEngine;

public interface IResetCamera
{
    void ResetCameraPosition();
}

public interface IMoveCamera
{
    void MoveCamera(Vector3 origin);
}

public interface ICamera : IMoveCamera, IResetCamera { }

public class CameraMove : MonoBehaviour, ICamera
{
    private Vector3 m_InitialPosition;

    private void Awake()
    {
        m_InitialPosition = transform.position;
    }

    /// <summary> Move camera to position.</summary>
    /// <param name="origin"> Position to move camera to.</param>
    public void MoveCamera(Vector3 origin)
    {
        transform.position += origin;
    }

    /// <summary> 
    ///     Reset camera position.
    /// </summary>
    public void ResetCameraPosition()
    {
        transform.position = m_InitialPosition;
    }     
}
