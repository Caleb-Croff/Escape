// Coded by Developer Jake -- https://www.youtube.com/developerjake
// Follow the Backrooms Game Lab (Part 7) to understand what this is for

using UnityEngine;
using UnityEngine.InputSystem;

public class KeyDoor : MonoBehaviour // This script should be on the Locked Door Trigger
{
    [Header("Attributes")]
   
    [Tooltip("The name of the key that is required.")] public string keyName = "";

    [Header("References")]

    public GameObject CursorHover; // The hover cursor that should show when the player is looking at the door

    public Animation Door;

    public AudioSource DoorOpenSound;

    private KeyManager km;

    private void Start()
    {
        km = FindFirstObjectByType<KeyManager>(); // Assign
    }

    private void OnMouseOver() // Activates when the player looks at the door
    {

        if ( PlayerCasting.DistanceFromTarget <= 4 ) // If the player IS close enough to the door..
        {

            CursorHover.SetActive(true);



            if (Keyboard.current.eKey.wasPressedThisFrame) // If the player presses E..
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

        else // If the player is NOT close enough to the door
        {

            CursorHover.SetActive(false);

        }
    }



    private void OnMouseExit() // Activates when the player looks away from the door
    {

        CursorHover.SetActive(false);

    }
}
