using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLevel : MonoBehaviour
{
    public GameObject SaveGameUI;

    [Range(1, 30)] public float timeBetweenSaves;

    void Awake()
    {
        InvokeRepeating(nameof(SaveGame), timeBetweenSaves, timeBetweenSaves);
    }

    public void SaveGame()
    {
        SaveGameUI.SetActive(true);

        PlayerPrefs.SetString("loaded level", SceneManager.GetActiveScene().name);

        PlayerPrefs.Save();

        Invoke(nameof(HideUI), 3);
    }

    public void HideUI()
    {
        SaveGameUI.SetActive(false);
    }
}
