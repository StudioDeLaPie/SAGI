using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubeDestroyer : NetworkBehaviour {

    public GameObject cube;

    private Vector3 cubeStartPos;

    private void Awake()
    {
        cubeStartPos = cube.transform.root.position;
    }

    // Use this for initialization
    void Start () {
        if (!isServer)
            Destroy(this);
	}

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.root.gameObject == cube)
        {
            StartCoroutine(DestroyCoroutine());
        }
    }

    private IEnumerator DestroyCoroutine()
    {
        float startTime = Time.time;
        cube.transform.position = new Vector3(0, -15, 0);
        cube.GetComponent<Weight>().CmdStop();
        while (Time.time < startTime + 3)
        {
            yield return null;
        }
        cube.GetComponent<Weight>().CmdIncreaseWeight();
        cube.transform.position = cubeStartPos;
    }
}
