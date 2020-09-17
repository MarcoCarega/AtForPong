using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Giant : MonoBehaviour
{
    private FSM fsm;

    private Transform destination;

    private List<GameObject> foodPoints;
    private GameObject chosenFood;

    // Start is called before the first frame update
    void Start()
    {
        FSMState initialState = makeStateMachine();
        fsm = new FSM(initialState);
        foodPoints = GameObject.FindGameObjectsWithTag("FoodPoint").ToList();
        chosenFood = foodPoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        PowerUp powerUp = other.GetComponent<PowerUp>();
        if(bullet!=null)
        {
            destination = bullet.shooter;
        }
        else if(powerUp!=null)
        {
            destination = powerUp.thrower;
        }
    }*/

    private FSMState makeStateMachine()
    {
        FSMState moveToFood = new FSMState();
        FSMState eatFood = new FSMState();
        FSMState chasePlayer = new FSMState();
        FSMState attackPlayer = new FSMState();
        moveToFood.AddStayAction(randomMovements);
        List<FSMAction> actions = new List<FSMAction>();
        moveToFood.AddTransition(new FSMTransition(isFoodNear, actions), eatFood);
        moveToFood.AddTransition(new FSMTransition(isHit, actions),chasePlayer);
        eatFood.AddTransition(new FSMTransition(isFoodFar, actions), moveToFood);
        eatFood.AddTransition(new FSMTransition(isHit, actions), chasePlayer);
        chasePlayer.AddTransition(new FSMTransition(isPlayerNear, actions), attackPlayer);
        attackPlayer.AddTransition(new FSMTransition(isPlayerFar, actions), chasePlayer);
        attackPlayer.AddTransition(new FSMTransition(isPlayerDied, actions), moveToFood);
        return moveToFood;
    }

    private bool isHit()
    {
        return destination != null;
    }

    private bool isFoodNear()
    {
        float distance = Math.Abs(Vector3.Distance(chosenFood.transform.position, transform.position));
        return distance <= 1;
    }

    private bool isFoodFar()
    {
        return !isFoodNear();
    }

    private bool isPlayerNear()
    {
        float distance = Math.Abs(Vector3.Distance(destination.transform.position, transform.position));
        return distance <= 1;
    }

    private bool isPlayerFar()
    {
        return isPlayerNear();
    }

    private bool isPlayerDied()
    {
        return destination != null;
    }

    private void randomMovements()
    {

    }

    private void chasePlayer()
    {

    }
}
