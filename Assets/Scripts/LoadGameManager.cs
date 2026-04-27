using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI LoadLevelText;

    [SerializeField] Button LoadButton;

    void Awake()
    {
        if (string.IsNullOrWhiteSpace(PlayerPrefs.GetString("loaded level", " ")))
        {
            LoadButton.interactable = false;
        }
        else
        {
            LoadButton.interactable = true;

            LoadLevelText.text = PlayerPrefs.GetString("loaded level", "");
        }
    }


    public void LoadGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("loaded level"));
    }
}
