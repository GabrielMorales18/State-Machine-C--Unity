using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSalon : StateBase<MinerClass>
{
    public override void Enter(MinerClass miner_ch)
    {
        miner_ch.currenLoc = MinerClass.LocationsM.saloon;
        Debug.Log("enter salon");
        // move to the mine
        //miner_ch.arriveloc=false;
        miner_ch.moveto(miner_ch.saloonLoc);
        enableState = false;  //todavia no llega a la mina
    }
    public override void Execute(MinerClass miner_ch)
    {
        Debug.Log("ejecute salon");
        // check the gold that the miner have
        Debug.Log("enable state in execute" + enableState);
        if (enableState)
        {
            // add code to get points for nuggets
            if (miner_ch.thirsty <= 0)
            {
                while (!miner_ch.my_FSM.ChangeState(new StateMine())) { };
            }
            
        }
        else
            return;
        return;
    }
    //***************************************************
    // execute when exit from state
    public override void Exit(MinerClass miner_ch)
    {
        Debug.Log("exit salon");
        enableState = false;
        // add a function to deactive animation digging
    }

}
