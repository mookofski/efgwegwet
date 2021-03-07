using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SC_PC_CONTROL : MonoBehaviour
{
    // Start is called before the first frame update



     float clamp(float v,float min,float ceil) 
        {
        if(v<min)
            {
                v=min;
            }
            if(v>ceil)
            {
                v=ceil;
            }
            return v;
        }


    public float speed;

    private Vector3 dir;
    private Transform T;
    private Transform CT;
    private Vector3 Mouse;

    Vector3 Mouse_p;
private Vector3 look;
public float sens;

    void Start()
    {
        look=new Vector3();
        Mouse_p=new Vector3();
        CT=GetComponentInChildren<Camera>().GetComponent<Transform>();
        T=GetComponent<Transform>();
        dir=new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        float spd=speed*Time.deltaTime;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            spd*=1.5f;
        }
       if(Input.GetKey(KeyCode.W))
        {
            dir+=spd*CT.forward;
        }
        if(Input.GetKey(KeyCode.A))
        {
            dir+=spd*CT.right*-1;
        }
        if(Input.GetKey(KeyCode.S))
        {
            dir+=spd*CT.forward*-1;
        }
        if(Input.GetKey(KeyCode.D))
        {
            dir+=spd*CT.right;
        }
        
        dir*=0.8f;
        T.position+=dir;

        Mouse=Input.mousePosition-Mouse_p;
        Mouse_p=Input.mousePosition;

        look+=Mouse*sens;

        look.y=clamp(look.y,-90,90);
        CT.rotation=Quaternion.Euler(look.y*-1,look.x,0f);
    }
}
