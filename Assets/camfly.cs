using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class camfly : MonoBehaviour
{

private LineRenderer line;

    private    Transform T;
    
    private Quaternion R;
    private Vector3 M;
    private Vector3 M_p;

    private Vector3 look;
    
private Camera C;
private Vector3[] LV;

public Quaternion Q;

    // Start is called before the first frame update
    void Start()
    {
        Q=new Quaternion();
      //  vt=new Vector3[3];
        LV=new Vector3[5];
        line=this.GetComponent<LineRenderer>();
        C=this.GetComponent<Camera>();
        M=Vector3.zero;
        M_p=Vector3.zero;
        T=GetComponent<Transform>();
        LV[0]=new Vector3(0,0,0);
        LV[1]=new Vector3(0,0,0);

        Debug.Log(C.aspect.ToString());
        
    }

public float sens=1;
        private Vector3 MOM;

private string db;

 public   float speed=2;
   public float deg=0.6f;

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


    public        float i1=1.0f;
      public  float i2=1.3f;
    // Update is called once per frame

float cos(float x)
{
    return (float)Math.Cos(x);
}
float sin(float x)
{
    return (float)Math.Sin(x);
}

float asin(float x)
{
    return (float)Math.Asin(x);
}
Quaternion etoq(Vector3 v)
{
v*=0.5f;


float cy=cos(v.y);
float sy=sin(v.y);

float cp=cos(v.x);
float sp=sin(v.x);

float cr=cos(v.z);
float sr=sin(v.z);


return new Quaternion
(//ya=z pt=y ro=x
//cir = 
   cr * sp * cy + sr * cp * sy,//y
   cr * cp * sy - sr * sp * cy,//z

   sr * cp * cy - cr * sp * sy,// x



   cr * cp * cy + sr * sp * sy//w

);
}
Vector3 v2r(Vector3 i)
{
    /*
rb.r_x=asin(-buf.y)*(180/pi);
rb.r_y=atan2(buf.x,buf.z)*(180/pi);

rb.r_z=0;


    */
    return new Vector3
    (
       (float) Math.Asin(-i.y),
        (float)Math.Atan2(i.x,i.z),
        0  
    );
}
Vector3 fromto(Vector3 from,Vector3 to)
{

    Vector3 r;

//heading = y
//bank=za
    double theta=Math.Acos(Vector3.Dot(from.normalized,to.normalized)/(1));
    Vector3 plane=Vector3.Cross(from,to).normalized;
double s=Math.Sin(theta);
double c=Math.Cos(theta);
double t=1-c;


r.z = (float)Math.Asin(plane.x * plane.y * t + plane.z * s) ;

    r.y=(float)Math.Atan2(
        plane.y * s- plane.x * plane.z * t ,
         1 - (plane.y*plane.y+ plane.z*plane.z ) * t);


r.x=(float) Math.Atan2(plane.x * s - plane.y * plane.z * t ,
 1 - (plane.x*plane.x + plane.z*plane.z) * t);



return r;    
}

Vector3 refl(Vector3 dir,Vector3 sur,float cur,float N)
{
//source
/*
http://www.euclideanspace.com/maths/geometry/rotations/conversions/angleToEuler/index.htm
*/
    Vector3 r;

//heading = y
//bank=za
    double theta=Math.Acos(Vector3.Dot(sur.normalized,dir.normalized)/(1));
    Vector3 plane=Vector3.Cross(sur,dir).normalized;
double s=Math.Sin(theta);
double c=Math.Cos(theta);
double t=1-c;


  //  r.z = (float)Math.Asin(plane.x * plane.y * t + plane.z * s) ;
r.z=0;
    r.y=(float)Math.Atan2(
        plane.y * s- plane.x * plane.z * t ,
         1 - (plane.y*plane.y+ plane.z*plane.z ) * t);

    r.x=(float) Math.Atan2(plane.x * s - plane.y * plane.z * t ,
    1 - (plane.x*plane.x + plane.z*plane.z) * t);

    r*=cur/N;
return -rot3d(sur,r);
}




Vector3 rotrev(Vector3 tgt,Vector3 r)
{
    Vector3 buf=tgt;
    Vector3 res=tgt;
    Vector3 R=r*-1;

    
	buf.x = (res.x * cos(R.y)) + (res.z * sin(R.y));
	buf.y = res.y;
	buf.z = (res.x * -sin(R.y)) + (res.z * cos(R.y));


    res=buf;

	buf.x = res.x;
	buf.y = (res.y * cos(R.x)) + (res.z * -sin(R.x));
	buf.z = (res.y * sin(R.x)) + (res.z * cos(R.x));

    return buf;
}

Vector3 rot3d(Vector3 tgt,Vector3 r)
{
    Vector3 buf=tgt;
    Vector3 res=tgt;

	buf.x = res.x;
	buf.y = (res.y * cos(r.x)) + (res.z * -sin(r.x));
	buf.z = (res.y * sin(r.x)) + (res.z * cos(r.x));


    res=buf;
	buf.x = (res.x * cos(r.y)) + (res.z * sin(r.y));
	buf.y = res.y;
	buf.z = (res.x * -sin(r.y)) + (res.z * cos(r.y));


    return buf;
}


public Vector3 unit;
public static int value;
        void Update()
    {
input();
RaycastHit hit;

for (var i = 0; i < 10; i++)
{
    if(Input.GetKeyDown(KeyCode.Alpha0+i))
{
    value=i;
}
}



if(    Physics.Raycast(
        C.transform.position,
        C.transform.forward,
        out hit,1000.0f))
        {

        LV[0]=C.transform.position;
        LV[1]=hit.point;

        Vector3 normal=hit.normal;
        Vector3 inc=(T.position-hit.point).normalized;

        Vector3 nr=v2r(normal);
        Vector3 ni=rotrev(inc,nr);

        Vector3 ir=v2r(ni);
  
        Quaternion pen=
        Quaternion.FromToRotation(normal,inc);


        //pen=v2r(pen);
        Vector3 logbuf1=Quaternion.FromToRotation(normal,inc).eulerAngles;
        Vector3 logbuf2=fromto(normal,inc);
        

        pen.x*=i1/i2;
        pen.z*=i1/i2;
        pen.y*=i1/i2;
        pen.w*=i1/i2;

        Vector3 ang=fromto(normal,inc);

        ang*=i1/i2;
        LV[2]=LV[1]+((rot3d(-normal,ang))*500.0f);
        LV[3]=LV[1];

   //     normal=rot3d(normal,ang);
       // normal=Quaternion.EulerAngles(ang)*normal;
      
           LV[4]=LV[1]+((refl(inc,normal,i1,i2))*500.0f);
 

if(Input.GetKeyDown(KeyCode.F)){

Debug.Log("rot"+logbuf1.ToString("F6"));
Debug.Log("pen"+logbuf2.ToString("F6"));
Debug.Log("dif"+(logbuf1-logbuf2).ToString("F6"));

Debug.Log(C.projectionMatrix.ToString("F6"));

Debug.Log(C.projectionMatrix.MultiplyPoint(new Vector3(0,0,1)).ToString("F6"));

}

        }
if(Input.GetKeyDown(KeyCode.F)){


Debug.Log(C.projectionMatrix.ToString("F6"));

Debug.Log(C.projectionMatrix.MultiplyPoint3x4(new Vector3(10,10  ,5)).ToString("F6"));

Debug.Log(C.cameraToWorldMatrix.ToString("F6"));
Debug.Log(C.cameraToWorldMatrix.MultiplyPoint3x4(new Vector3(10,0  ,5)).ToString("F6"));
}




if(LV.Length>0){
for (int i =0 ; i < Math.Min(LV.Length,5); i++)
{
    line.SetPosition(i,LV[i]);
}
line.startColor=new Color(1,0,0,1);
line.endColor=new Color(0,1,0,1);

}


}




private void input()
{
if(Input.GetKeyDown(KeyCode.R))
        {
            look=new Vector3();
        }
    
   
        M=Input.mousePosition-M_p;
        M_p=Input.mousePosition;

        look+=M*sens;

        look.y=clamp(look.y,-45,45);
        
        float spb=speed*Time.deltaTime;

            if(Input.GetKey(KeyCode.LeftShift))
            {
                spb*=2.0f;
            }


      T.rotation=Quaternion.Euler(look.y*-1,look.x,0f);


        if(Input.GetKey(KeyCode.W))
        {
            MOM+=T.forward*spb;
        }
              if(Input.GetKey(KeyCode.A))
        {
            MOM-=T.right*spb;
        }
              if(Input.GetKey(KeyCode.S))
        {
            MOM-=T.forward*spb;
        }
              if(Input.GetKey(KeyCode.D))
        {
            MOM+=T.right*spb;
        }

MOM*=deg;

         T.position+=MOM;


}

}




