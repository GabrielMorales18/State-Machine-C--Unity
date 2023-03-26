using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveVel : MonoBehaviour {

    // general data
    public Vector3 vc_Velocity;
    public Vector3 vc_Heading;
    Vector3 vn_Velocity;
    public float s_rotSpeed=20.0f;

    public float s_MaxSpeed=10.0f;
    public float s_MinSpeed = 1.0f;

    private Vector3 newPosition;

    public GameObject TargetSeek;
    public GameObject TargetFlee;
    public GameObject TargetPursuit;
    public GameObject TargetEvade;

  
    public bool OnSeek = false;
    public bool OnFlee = false;
    public bool OnPursuit = false;
    public bool OnWander = false;
    public bool OnEvade = false;
    public bool OnArrival = false;
    public bool OnOfPursuit = false;

    public float s_panicDist;

    public float wanderRadius;
    public float jitter;
    public float distanceWander;
    private Vector3 targetWander = new Vector3(1, 0, 1);
    // Use this for initialization
    void Start () {
       // s_MaxSpeed = 8.0f;
        s_MinSpeed = 0.2f;
        s_panicDist = 30.0f;
        vn_Velocity = new Vector3(0.0f, 0.0f, 0.0f);
        vc_Velocity = new Vector3(0.0f, 0.0f, 0.0f);
        

    }
	
	// Update is called once per frame
	void Update () {
        
        vn_Velocity = Vector3.zero;

        if (OnSeek )
        { if (Vector3.Distance(TargetSeek.transform.position, transform.position) > 1.0)
                vn_Velocity = vn_Velocity + Seek(TargetSeek.transform.position);
            else
                //vn_Velocity = Vector3.zero;
                vn_Velocity = vc_Velocity * -1.0f;
        }

        if (OnFlee)
        {
            vn_Velocity = vn_Velocity + Flee(TargetFlee.transform.position);
        }

        if (OnPursuit)
        {
            vn_Velocity = vn_Velocity + Pursuit(TargetPursuit.GetComponent<moveVel>().s_MaxSpeed);
        }

        if (OnWander)
        {
            vn_Velocity = vn_Velocity + Wander();
        }
        if (OnEvade)
        {
            vn_Velocity = vn_Velocity - Evade(TargetEvade.GetComponent<moveVel>().s_MaxSpeed);
        }
        if (OnOfPursuit)
        {
            vn_Velocity = vn_Velocity + Ofpursuit(TargetPursuit.GetComponent<moveVel>().s_MaxSpeed);
        }

        //**********************************************************

        vc_Velocity += vn_Velocity;
        vc_Velocity = Vector3.ClampMagnitude(vc_Velocity, s_MaxSpeed);
        if (OnArrival && OnSeek)
        {
            vc_Velocity = Arrival(vc_Velocity);
        }
        newPosition = transform.position + (vc_Velocity * Time.deltaTime);

        if(vc_Velocity.magnitude>s_MinSpeed)
             transform.position = newPosition;
       
        // update the direction of the boid
        vc_Heading = vc_Velocity.normalized;

        //****************************************

        float angle = Vector3.SignedAngle(transform.forward, vc_Heading, Vector3.up);
        //Debug.Log(angle);
        float rotAngle = 0.0f;

        if (angle < -0.1f)
            rotAngle = Time.deltaTime * -1.0f * s_rotSpeed;
        if (angle > 0.1f)
            rotAngle = Time.deltaTime * s_rotSpeed;
        if (angle >= -0.1f && angle <= 0.1f)
        {
            if (Vector3.Dot(transform.forward, vc_Heading) >= 0.9)
                rotAngle = 0.0f;
            if (Vector3.Dot(transform.forward, vc_Heading) <= -0.9)
                rotAngle = 10.0f;
        }

        //Debug.Log("rotAngle" + rotAngle);
        transform.Rotate(0.0f, rotAngle, 0.0f, Space.Self);
        //*****************************************
            }
    //******************************************************************

    public Vector3 Seek(Vector3 targetseek)
    {
        Vector3 direction;
       
       direction =  targetseek-transform.position;
        direction.y = 0;

        if (direction.magnitude < 1.0f )
        {
            
            return (Vector3.zero);
        }
        direction.Normalize();
        Vector3 DesiredVelocity = direction * s_MaxSpeed;
        DesiredVelocity = Vector3.ClampMagnitude(DesiredVelocity, s_MaxSpeed);

        return (DesiredVelocity - vc_Velocity);


    }

    //******************************************************************
    public Vector3 Flee(Vector3 targetFlee)
    {
        Vector3 direction;

        direction =  transform.position-targetFlee;
        direction.y = 0;

        if (direction.magnitude>s_panicDist)
        {
           
            return (Vector3.zero);
        }
        direction.Normalize();
        Vector3 DesiredVelocity = direction * s_MaxSpeed;
        DesiredVelocity = Vector3.ClampMagnitude(DesiredVelocity, s_MaxSpeed);

        return (DesiredVelocity - vc_Velocity);


    }

    //******************************************************************
    ///  ************** function pursuit*********
    public Vector3 Pursuit(float targetSpeed)
    {
        float lookAheadTime = Vector3.Magnitude(TargetPursuit.transform.position - this.transform.position) / (targetSpeed + this.s_MaxSpeed);
        Vector3 futurePosition = TargetPursuit.transform.position + (TargetPursuit.GetComponent<moveVel>().vc_Velocity * lookAheadTime);
        return Seek(futurePosition);
    }
    //*******************************************************************
    ///  ************** function evade*********
    public Vector3 Evade(float targetSpeed)
    {
        if(Vector3.Dot(TargetEvade.GetComponent<moveVel>().vc_Heading,vc_Heading)<0.5 && Vector3.Dot(TargetEvade.GetComponent<moveVel>().vc_Heading,vc_Heading)> -0.5){
            Vector3 futurePosition = TargetEvade.transform.position + (TargetEvade.GetComponent<moveVel>().vc_Velocity * 5);
            return Seek(futurePosition);
        }
        return new Vector3(0, 0, 0);
    }
    //*******************************************************************
    public Vector3 Wander()
    {   
        targetWander = Vector3.Normalize(targetWander);
        Vector3 randomVector = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        randomVector = Vector3.Normalize(randomVector) * jitter;
        targetWander += randomVector;
        targetWander = Vector3.Normalize(targetWander) * wanderRadius;
        Vector3 despCircle = vc_Heading * distanceWander;
        targetWander = despCircle + targetWander;
        return targetWander;
    }

    //*************************************************************************
    public Vector3 Arrival(Vector3 velocity)
    {
        if (Vector3.Distance(this.transform.position, TargetSeek.transform.position) < 5)
        {
            velocity *= Vector3.Distance(this.transform.position, TargetSeek.transform.position) / 5;
        }
        return velocity;
    }

    public Vector3 Ofpursuit(float targetSpeed)
    {
        if (Vector3.Distance(TargetPursuit.transform.position, this.transform.position) > 3)
        {
            float lookAheadTime = Vector3.Magnitude(TargetPursuit.transform.position - this.transform.position) / (targetSpeed + this.s_MaxSpeed);
            Vector3 futurePosition = TargetPursuit.transform.position + (TargetPursuit.GetComponent<moveVel>().vc_Velocity * lookAheadTime);
            return Seek(futurePosition);
        }
        return new Vector3(0, 0, 0);
    }
    //*************************************************************************
    public Vector3 ObstacleAvoidance()
    {
        Vector3 DesiredVelocity;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10) && hit.transform.gameObject.tag == "Obstacle")
        {
            Vector3 multi = Vector3.Cross(Vector3.up + transform.position, transform.forward);
            multi.Normalize();
            Vector3 newNormal = hit.normal - hit.transform.position;
            if (Vector3.Dot(hit.normal, multi) < 0)
            {
                multi = multi * (-1);
            }
            //Debug.DrawRay(hit.transform.position, hit.normal, Color.blue);
            Vector3 width = Vector3.Scale(transform.localScale, transform.GetComponent<Collider>().bounds.size);
            Debug.Log(newNormal);
            Vector3 newPosition = hit.point + newNormal * width.magnitude * 1.5f;
            DesiredVelocity = newPosition - transform.position;
        }
        else
        {
            DesiredVelocity = vc_Velocity;
        }

        return (DesiredVelocity - vc_Velocity);
    }
    //*************************************************************************
    /*
    void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, vc_Heading * 10.0f + transform.position, Color.red);
        Debug.DrawLine(transform.position, transform.forward * 10.0f + transform.position, Color.green);
 
    }
    */
}
