using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject camera1;
    public GameObject camera2;
    public GameObject camera3;

    public void ToggleCamera1()
    {
        camera1.gameObject.SetActive(false);
        camera1.gameObject.SetActive(true);
    }

    public void ToggleCamera2()
    {
        camera2.gameObject.SetActive(false);
        camera2.gameObject.SetActive(true);
    }

    public void ToggleCamera3()
    {
        camera3.gameObject.SetActive(false);
        camera3.gameObject.SetActive(true);
    }
}
