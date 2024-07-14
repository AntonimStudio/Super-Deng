using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuButton : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;

    // Update is called once per frame
    public void TurnOff()
    {
        foreach(GameObject obj in gameObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
    public void TurnOn()
    {
        foreach (GameObject obj in gameObjects)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
}
