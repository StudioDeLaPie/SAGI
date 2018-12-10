using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationDevelopMode : MonoBehaviour
{
    [SerializeField] List<GameObject> Debugs;
    bool active;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {            
            if (Input.GetKey(KeyCode.KeypadEnter))
            {
                active = true;
                Debug.Log("Enter");
            }
            else
                active = false;

            for (int i = 0; i < Debugs.Count; i++)
            {
                Debugs[i].SetActive(active);
            }
        }
    }
}
