using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Animator>().SetTrigger("FruitReached");
    }

}
