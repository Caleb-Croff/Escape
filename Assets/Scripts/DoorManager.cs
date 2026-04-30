using UnityEngine;
using UnityEngine.InputSystem;

public class DoorManager : MonoBehaviour
{
    public GameObject CursorHover;

    public Animation Door;

    public AudioSource DoorOpenSound;

    private void OnMouseOver()
    {

        if ( PlayerCasting.DistanceFromTarget < 4 )
        {

            CursorHover.SetActive(true);



            if (Keyboard.current.eKey.wasPressedThisFrame)
            {

                GetComponent<BoxCollider>().enabled = false;

                Door.Play();

                DoorOpenSound.Play();

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
