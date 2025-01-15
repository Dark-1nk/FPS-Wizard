using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public class DoorEntry
    {
        public GameObject Door;
        public bool isRed;
        public bool isGreen;
        public bool isPink;
    }

    public List<DoorEntry> Doors;

    public void OpenMe(string color)
    {
        if (color == "Red")
        {
            foreach (DoorEntry entry in Doors)
            {
                if (entry.isRed)
                {
                    gameObject.SetActive(false);
                }
            }
        }
        if (color == "Pink")
        {
            foreach (DoorEntry entry in Doors)
            {
                if (entry.isPink)
                {
                    gameObject.SetActive(false);
                }
            }
        }
        if (color == "Green")
        {
            foreach (DoorEntry entry in Doors)
            {
                if (entry.isGreen)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
