using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticlesManager : MonoBehaviour {

    public ParticleSystem groundParticles;
    public ParticleSystem roofParticles;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].point.y < transform.position.y)
        {
            groundParticles.Stop();
            groundParticles.Play();
        }
        else if (collision.contacts[0].point.y > transform.position.y)
        {
            roofParticles.Stop();
            roofParticles.Play();
        }
    }
}
