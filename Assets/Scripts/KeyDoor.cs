using UnityEngine;
using UnityEngine.InputSystem;

public class KeyDoor : MonoBehaviour
{
    [Header("Attributes")]
   
    [Tooltip("The name of the key that is required.")] public string keyName = "";

    [Header("References")]

    public GameObject CursorHover;

    public Animation Door;

    public AudioSource DoorOpenSound;

    private KeyManager km;

    private void Start()
    {
        km = FindFirstObjectByType<KeyManager>();
    }

    private void OnMouseOver()
    {

        if ( PlayerCasting.DistanceFromTarget <= 4 )
        {

            CursorHover.SetActive(true);



            if (Keyboard.current.eKey.wasPressedThisFrame)
            {

                string foundKey = null;
                foreach (string key in km.keysInInventory)
                {
                    if (key.Trim().ToLower() == keyName.Trim().ToLower())
                    {
                        foundKey = key;
                        break;
                    }
                }

                if (foundKey != null)
                {
                    GetComponent<BoxCollider>().enabled = false;
                    Door.Play();
                    DoorOpenSound.Play();
                    km.keysInInventory.Remove(foundKey);
                }
            }

        }

        else
        {

            CursorHover.SetActive(false);

        }
    }



    private void OnMouseExit()
    {

        CursorHover.SetActive(false);

    }
}
