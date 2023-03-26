using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerClass : CharacClass
{
    public FSM_class<MinerClass> my_FSM;
    public LocationsM currenLoc;     


    // variables for properties
    public bool despositDone = false;
    public bool drinkingDone = false;
    public bool restingDone = false;
    // how many nuggets the miner has in his pocket
    public int numNuggets;
    // how much money the miner has deposited in the bank
    public int moneyBank;
    // control of thirst
    public int thirsty;
    // control fo fatigue
    public int fatigue;


    // locations
    public GameObject mineLoc;
    public GameObject banckLoc;
    public GameObject saloonLoc;
    public GameObject homeLoc;

    public bool getnugget = false;
    public bool arriveloc = false;
    public bool moving = false;
    public bool runmoveTo = false;

    // times for each action
    float timetired = 0.0f;
    float timethirsty = 0.0f;

    float timerest = 0.0f;
    float timemine = 0.0f;
    float timeDeposit = 0.0f;
    float timeDrinking = 0.0f;
    //*****************************************************
    public bool ChangeState(StateBase<MinerClass> newState)
    {
        my_FSM.ChangeState(newState);
        return (true);
    }


    //********************************************
    void Start()
    {
        SetID((int)CharacClass.enumCharac.Bob);

        numNuggets = 0;
        moneyBank = 0;
        fatigue = 0;
        thirsty = 0;

        GameObject theminer = this.gameObject;
        //initialize moving
        moveCod = this.GetComponent<moveVel>();
        moveCod.OnFlee = false;
        moveCod.OnSeek = false;

        //initialize FSM
       my_FSM = new FSM_class<MinerClass>();

        my_FSM.SetOwner(this);
        my_FSM.Begin(new StateMine());


        currenLoc = LocationsM.mine;




    }

    //*********************************************

    void Update()
    {
        my_FSM.UpdateMachine();

        
        if (Arrive()) //arrive to destination
        {
            //Debug.Log("arrive");
            my_FSM.CurrentState.enableState = true;
         }
        updateInputs();
    }

    //*********************************************
    void updateInputs()
    {
        timethirsty += Time.deltaTime;

        if (currenLoc != LocationsM.home)
        {
            timetired += Time.deltaTime;
        }
    
        if (timetired > 0.2f * moveCod.s_MaxSpeed)
        {
            fatigue++;
            timetired = 0.0f;
        }
        if (timethirsty > 0.2f * moveCod.s_MaxSpeed)
        {
            thirsty++;
            timethirsty = 0.0f;
        }
       
        if (currenLoc == LocationsM.mine && my_FSM.CurrentState.enableState)
        {
            timemine += Time.deltaTime;
            if (timemine > 0.25f * moveCod.s_MaxSpeed)
            {
                numNuggets += 5;
                timemine = 0.0f;
            }
        }
        if (currenLoc == LocationsM.saloon && my_FSM.CurrentState.enableState)
        {
            timeDrinking += Time.deltaTime;
            if (timeDrinking > 0.5f * moveCod.s_MaxSpeed)
            {
                done_drinking();
                timeDrinking = 0;
            }
        }
        if (currenLoc == LocationsM.bank && my_FSM.CurrentState.enableState)
        {
            timeDeposit += Time.deltaTime;
            if (timeDeposit > 0.5f * moveCod.s_MaxSpeed)
            {
                done_deposit();
                timeDeposit = 0;
            }
        }
        if (currenLoc == LocationsM.home && my_FSM.CurrentState.enableState)
        {
            timerest += Time.deltaTime;
            if (timerest > 0.5f * moveCod.s_MaxSpeed)
            {
                done_resting();
                timerest = 0;
            }
        }


        Debug.Log("fatigue " + fatigue + " thirsty " + thirsty);
        Debug.Log("nuggets " + numNuggets + " banco " + moneyBank);

    }
    //***********************************************
        
    public void done_deposit()
    {
        moneyBank += (numNuggets * 20);
        numNuggets = 0;
        despositDone = true;
    }
    public void done_resting()
    {
        fatigue = 0;
        restingDone = true;
    }
    public void done_drinking()
    {
        thirsty = 0;
        drinkingDone = true;
    }

    public void moveto(GameObject location)
    {
        moveCod.TargetSeek = location;
        moveCod.OnSeek = true;

        //moveCod.OnWander = true;
       
    }

    public bool Arrive()
    {
        GameObject target;
        switch (currenLoc)
        {
            case LocationsM.home: target = homeLoc; break;
            case LocationsM.bank: target = banckLoc; break;
            case LocationsM.saloon: target = saloonLoc; break;
            case LocationsM.mine: target = mineLoc; break;
            default:
                target = mineLoc; break;
        }
        Vector3 dist = transform.position - target.transform.position;
        if (dist.magnitude < 1.0f)
            return true;
        else
            return false;
    }

}
