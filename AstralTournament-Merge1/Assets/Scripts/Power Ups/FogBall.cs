using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogBall : ThrowablePowerUp
{
    public int timer;

    public override void OnThrow(Vector3 force)
    {
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.AddForce(force);
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other is MeshCollider || other is TerrainCollider)
        {
            Vector3 scale = Vector3.zero;
            transform.localScale = scale;
            StartCoroutine("Fog");           
        }
    }

    private IEnumerator Fog()
    {
        GameObject fog = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Particles/Fog"));
        Vector3 position = transform.position;
        position.y += 50;
        fog.transform.position = position;
        yield return new WaitForSeconds(timer);
        fog.GetComponent<ParticleSystem>().Pause();
        StartCoroutine("AwaitEnd");
        Destroy(fog);
        Destroy(gameObject);
    }

    private IEnumerator AwaitEnd()
    {
        yield return new WaitForSeconds(5);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
