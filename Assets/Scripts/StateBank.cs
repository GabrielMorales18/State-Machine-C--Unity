using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBank : StateBase<MinerClass>
{
    public override void Enter(MinerClass miner_ch)
    {
        miner_ch.currenLoc = MinerClass.LocationsM.bank;
        Debug.Log("enter bank");
        // move to the mine
        //miner_ch.arriveloc=false;
        miner_ch.moveto(miner_ch.banckLoc);
        enableState = false;  //todavia no llega a la mina
    }
    public override void Execute(MinerClass miner_ch)
    {
        Debug.Log("ejecute bank");
        // check the gold that the miner have
        Debug.Log("enable state in execute" + enableState);
        if (enableState)
        {
            // add code to get points for nuggets
            if (miner_ch.numNuggets <= 0)
            {
                if (miner_ch.fatigue >= 10)
                {
                    while (!miner_ch.my_FSM.ChangeState(new StateHome())) { };
                } else
                {
                    while (!miner_ch.my_FSM.ChangeState(new StateMine())) { };
                }
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
        Debug.Log("exit bank");
        enableState = false;
        // add a function to deactive animation digging
    }

}
