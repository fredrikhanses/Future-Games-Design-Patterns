using UnityEngine;

public class MoveCamera : MonoBehaviour
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
