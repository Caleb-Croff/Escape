using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
  private void Start()
  {
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
  }

  public void ChangeScene(string sceneName)
  {
    SceneManager.LoadScene(sceneName);
  }
}
