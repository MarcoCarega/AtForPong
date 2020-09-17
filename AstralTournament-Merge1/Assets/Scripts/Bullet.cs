using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float attack;
    public Type attackType;

    public GameObject hitPrefab;

    internal NetworkInstanceId shooter;

    private void OnTriggerEnter(Collider collider)
    {
        NetworkVehicle net = collider.GetComponent<NetworkVehicle>();
        if (net != null)
        {
            TypeManager typeManager = new TypeManager();
            NewPlayerController player = net.GetComponent<NewPlayerController>();
            int damage = Mathf.CeilToInt(player.defense - attack * typeManager.EffectValue(attackType, player.type));
            if (damage >= 0) net.health -= damage;
        }
        Bullet bullet = collider.GetComponent<Bullet>();
        if (bullet != null) return;
        if (collider.CompareTag("Boss"))
        {
            collider.GetComponent<Boss>().takeDamage(attack, shooter);
            Debug.Log("hitting boss");
        }
        StartCoroutine("HitBullet");
    }

    private IEnumerator HitBullet()
    {
        GameObject hit = Instantiate<GameObject>(hitPrefab);
        hit.transform.position = transform.position;
        yield return new WaitForSeconds(0.1f);
        Destroy(hit);
        Destroy(gameObject);
        
    }

    public void setShooter(NetworkInstanceId IDShooter)
    {
        Debug.Log(IDShooter);
        shooter = IDShooter;
    }
}
