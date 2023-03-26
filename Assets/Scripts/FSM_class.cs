using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_class<T> where T : CharacClass
{
    T Owner;

    public StateBase<T> CurrentState;
    public StateBase<T> GlobalState;
    public StateBase<T> PreviousState;


    //**********************************************************************************************
    public void UpdateMachine()
    {
        //if (GlobalState != null)
        //{
        //    GlobalState.Execute(Owner);
        //}


        if (CurrentState != null)
        {
            CurrentState.Execute(Owner);
        }

    }
    //**********************************************************************************************

    public void SetOwner(T owner)
    {
        Owner = owner;
    }

    //**********************************************************************************************

    public bool ChangeState(StateBase<T> newState)
    {
        //verify the state are valid
        if (newState == null)
        {
            return false;
        }
        // call the exit of the current state
        CurrentState.Exit(Owner);
        //currentState = null;

        PreviousState = CurrentState;
        // change of state
        CurrentState = newState;

        // call the entry method of the newstate
        CurrentState.Enter(Owner);

        return true;
    }
    //**********************************************************************************************
    public void RevertToPreviousState()
    {
        CurrentState.enableState = false;
        CurrentState.Exit(Owner);
        CurrentState = PreviousState;

        CurrentState.Enter(Owner);

    }
    //**********************************************************************************************

    public bool Begin(StateBase<T> initialST)
    {
        if (initialST == null)
        {
            return false;
        }
        CurrentState = initialST;
        // PreviousState = initialST;
        CurrentState.Enter(Owner);
        return true;
    }
    //**********************************************************************************************
    public bool SetGlobalState(StateBase<T> globalST)
    {
        if (globalST == null)
        {
            return false;
        }
        GlobalState = globalST;
        return true;
    }
    //**********************************************************

}
