using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate bool FSMCondition();

public delegate void FSMAction();

public class FSMTransition
{
    private FSMCondition condition;
    public FSMCondition Condition
    {
        get
        {
            return condition;
        }

        set
        {
            this.condition = value;
        }
    }

    private List<FSMAction> actions;

    public FSMTransition(FSMCondition condition,List<FSMAction> actions)
    {
        this.condition = condition;
        this.actions = actions;

    }

    public void Execute()
    {
        if (actions != null)
            foreach (FSMAction action in actions)
                action();
    }
}

public class FSMState
{
    private List<FSMAction> enters;
    private List<FSMAction> stays;
    private List<FSMAction> exits;

    private Dictionary<FSMTransition, FSMState> links;

    public FSMState()
    {
        enters = new List<FSMAction>();
        stays = new List<FSMAction>();
        exits = new List<FSMAction>();
    }

    public void AddEnterAction(FSMAction action)
    {
        enters.Add(action);
    }

    public void AddStayAction(FSMAction action)
    {
        enters.Add(action);
    }

    public void AddExitAction(FSMAction action)
    {
        enters.Add(action);
    }

    public void AddTransition(FSMTransition transition, FSMState target)
    {
        if (links.ContainsKey(transition)) links[transition] = target;
        else links.Add(transition, target);
    }

    public FSMTransition VerifyTransition()
    {

        foreach (FSMTransition transition in links.Keys)
            if (transition.Condition())
                return transition;
        return null;
    }

    public FSMState NextState(FSMTransition transition)
    {
        return links[transition];
    }

    public void Enter()
    {
        foreach (FSMAction enter in enters)
            enter();
    }

    public void Stay()
    {
        foreach (FSMAction stay in stays)
            stay();
    }

    public void Exit()
    {
        foreach (FSMAction exit in exits)
            exit();
    }

    
}

public class FSM
{
    public FSMState current;

    public FSM(FSMState state)
    {
        current = state;
        current.Enter();
    }

    public void Update()
    {
        FSMTransition transition = current.VerifyTransition();
        if (transition != null)
        {
            current.Exit();
            transition.Execute();
            current = current.NextState(transition);
            current.Enter();
        }
        else current.Stay();
    }
}

