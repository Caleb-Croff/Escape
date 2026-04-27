using UnityEngine;
using UnityEngine.InputSystem;

public class Key : MonoBehaviour
{
    [Tooltip("The name of the key. This corresponds with the key on the door")] public string keyName;

    public GameObject HoverIcon;

    [Header("(Optional)")]
    [Tooltip("(Optional.)")] public AudioClip CollectAudio;

    public void ShowHover(bool show)
    {
        HoverIcon.SetActive(show);
    }

    public void TryCollect()
    {
        if (CollectAudio != null)
            FindFirstObjectByType<KeyManager>().GetComponent<AudioSource>().PlayOneShot(CollectAudio);

        FindFirstObjectByType<KeyManager>().keysInInventory.Add(keyName);

        HoverIcon.SetActive(false);
        Destroy(gameObject);
    }
}
