
using Cinemachine;

public class CameraController : Singleton<CameraController>
{
    private CinemachineVirtualCamera _cineMachineVirtualCamera;

    private void Start()
    {
        SetPlayerCameraFollow();
    }

    public void SetPlayerCameraFollow()
    {
        _cineMachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        _cineMachineVirtualCamera.Follow = PlayerController.Instance.transform;
    }
}
