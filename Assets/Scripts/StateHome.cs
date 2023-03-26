using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHome : StateBase<MinerClass>
{
    public override void Enter(MinerClass miner_ch)
    {
        miner_ch.currenLoc = MinerClass.LocationsM.home;
        Debug.Log("enter home");
        // move to the mine
        //miner_ch.arriveloc=false;
        miner_ch.moveto(miner_ch.homeLoc);
        enableState = false;  //todavia no llega a la mina
    }
    public override void Execute(MinerClass miner_ch)
    {
        Debug.Log("ejecute home");
        // check the gold that the miner have
        Debug.Log("enable state in execute" + enableState);
        if (enableState)
        {
            // add code to get points for nuggets
            if (miner_ch.fatigue <= 0)
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
        Debug.Log("exit home");
        enableState = false;
        // add a function to deactive animation digging
    }

}
