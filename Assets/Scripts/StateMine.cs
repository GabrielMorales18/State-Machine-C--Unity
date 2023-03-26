using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMine : StateBase<MinerClass>
{
    //public  bool enableState=false;
    // action to execute when enter the state
    public override void Enter(MinerClass miner_ch)
    {
        miner_ch.currenLoc = MinerClass.LocationsM.mine;
        Debug.Log("enter mine");
        // move to the mine
        //miner_ch.arriveloc=false;
        miner_ch.moveto(miner_ch.mineLoc);
        enableState = false;  //todavia no llega a la mina
    }
    //*************************************************
    // is call by update miner function
    public override void Execute(MinerClass miner_ch)
    {
        Debug.Log("ejecute mine");
        // check the gold that the miner have
        Debug.Log("enable state in execute" + enableState);
        if (enableState)
        {
            // add code to get points for nuggets
            if (miner_ch.numNuggets >= 20)
            {
                miner_ch.despositDone = false;
                while (!miner_ch.my_FSM.ChangeState(new StateBank())) { };
            }
            else
            {
                //Debug.Log("tired in ");
                if (miner_ch.fatigue >= 10)
                {
                    Debug.Log("tired");
                    miner_ch.restingDone = false;
                    while (!miner_ch.my_FSM.ChangeState(new StateHome())) { };
                }
                else
                {
                    Debug.Log("check thirsty");
                    if (miner_ch.thirsty >= 7)
                    {
                        Debug.Log("thirsty");
                        miner_ch.drinkingDone = false;
                        while (!miner_ch.ChangeState(new StateSalon())) { };
                    }
                }
            }  // se queda en el mismo estado
        }
        else
            return;
        return;
    }
    //***************************************************
    // execute when exit from state
    public override void Exit(MinerClass miner_ch)
    {
        Debug.Log("exit mine");
        enableState = false;
        // add a function to deactive animation digging
    }


}
