using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationDevelopMode : MonoBehaviour
{
    [SerializeField] List<GameObject> Debugs;
    bool active;

    private void Start()
    {
        if (FindObjectsOfType<ActivationDevelopMode>().Length <= 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Keypad1))
        {            
            if (Input.GetKey(KeyCode.KeypadEnter))            
                active = true;                            
            else
                active = false;

            for (int i = 0; i < Debugs.Count; i++)
            {
                Debugs[i].SetActive(active);
            }
        }
    }
}
