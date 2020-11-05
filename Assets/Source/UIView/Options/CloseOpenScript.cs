using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseOpenScript : MonoBehaviour
{
    public GameObject GO;

    public void CloseOpen()
    {
        GO.SetActive(!GO.activeSelf);
    }
}
