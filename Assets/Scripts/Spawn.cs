using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject Saloon;
    public GameObject Home;
    public GameObject Mine;
    public GameObject Bank;
    public GameObject Miner;
    public int maxMiners;
    int minerCont = 0;
    float time = 2;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = 2;
            if(maxMiners > minerCont)
            {
                minerCont++;
                GameObject minerObj = Instantiate(Miner, new Vector3(this.transform.position.x, 0.5f, this.transform.position.z), Quaternion.identity);
                MinerClass minerScript = minerObj.GetComponent<MinerClass>();
                minerScript.mineLoc = Mine;
                minerScript.homeLoc = Home;
                minerScript.banckLoc = Bank;
                minerScript.saloonLoc = Saloon;
            }
        }
    }
}
