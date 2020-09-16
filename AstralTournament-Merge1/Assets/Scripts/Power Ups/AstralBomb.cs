using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AstralBomb : ThrowablePowerUp
{
    public int damage;
    public float forceExplosion;
    public float radiusExplosion;
    private bool explosionFlag;
    public int timer;
    private bool isTriggered;

    private CharacterController controller;

    public override void OnThrow(Vector3 force)
    {
        print("BOMB FORCE: " + force);
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.AddForce(force);
        explosionFlag = false;
        isTriggered = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        print("COLLIDED");
        PowerUp up = collider.GetComponent<PowerUp>();
        if (up != null) return;
        NetworkVehicle net = collider.GetComponent<NetworkVehicle>();
        if (net != null)
        {
            Rigidbody rigid = GetComponent<Rigidbody>();
            Vector3 position = transform.position;
            StartCoroutine("instantExplode");
            net.health -= damage;
        }
        else if (collider is MeshCollider || collider is TerrainCollider)
        {
            explosionFlag = true;
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (explosionFlag)
        {
            print("FLAG TRUE");
            if(!isTriggered) StartCoroutine("explosionCoroutine");
            isTriggered = true;
        }
        else print("FLAG FALSE");
    }

    private IEnumerator explosionCoroutine()
    {
        
        yield return new WaitForSeconds(timer);
        Rigidbody rigid = GetComponent<Rigidbody>();
        Vector3 position = transform.position;
        transform.localScale = Vector3.zero;
        rigid.AddExplosionForce(forceExplosion, position, radiusExplosion);
        GameObject explosion = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Particles/Explosion"));
        explosion.transform.position = transform.position;
        yield return new WaitForSeconds(1);
        Destroy(explosion);
        Destroy(gameObject);
        
    }

    private IEnumerator instantExplode()
    {
        GameObject explosion = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Particles/Explosion"));
        explosion.transform.position = transform.position;
        yield return new WaitForSeconds(1);
        Destroy(explosion);
        Destroy(gameObject);
    }
}
