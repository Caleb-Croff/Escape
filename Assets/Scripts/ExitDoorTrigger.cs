using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorTrigger : MonoBehaviour
{
    public GameObject WinUI;


    public void OnTriggerEnter(Collider other)
    {
        this.GetComponent<BoxCollider>().enabled = false;

        FindFirstObjectByType<FPSController>().enabled = false;
    
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        WinUI.SetActive(true);
    }

}
