using UnityEngine;
using System.IO;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;


public class NewBehaviourScript : MonoBehaviour
{
public GameObject cam;

/*vec4 v[3]={{vec4(0,0,0,0)},{vec4(10,0,0,0)},{vec4(0,10,0,0)}};
vec4 U=v[1]-v[0];
vec4 V=v[2]-v[0];
vec4 P=v[0];
vec4 N=vec4::cross(U,N);
vec4 W=(camera->rot.getfoward()+camera->coords)-v[0];
float r1=vec4::dot(N,(v[0]-camera->coords))/
vec4::dot(N,camera->rot.getfoward());

double s1=(
vec4::dot(U,V)*vec4::dot(W,V)-
vec4::dot(V,V)*vec4::dot(W,U))/
(vec4::dot(U,V)*vec4::dot(U,V))-
vec4::dot(U,U)*vec4::dot(V,V);


double t1=(
vec4::dot(U,V)*vec4::dot(W,V)-
vec4::dot(U,U)*vec4::dot(W,U))/
(vec4::dot(U,V)*vec4::dot(U,V))-
vec4::dot(U,U)*vec4::dot(V,V);

vec4 T=v[0]+(U*s1)+(V*t1);
wall.transform.coords=T;
*/
Camera cc;
public Vector3[] v;

Transform CamT;
    // Start is called before the first frame update
    void Start()
    {

    CamT=cam.GetComponent<Transform>();
  
    }

    // Update is called once per frame


    public GameObject Box;





   public Vector3 Origin;
  public      Vector3 To;
public Vector3  T;

private String deb;
public Vector3 Edge1,Edge2,P,N;

private bool O;
    void Update()
    {
Debug.DrawLine(v[0],v[1]);
Debug.DrawLine(v[1],v[2]);
Debug.DrawLine(v[2],v[0]);
O=false;
if(Input.GetKeyDown(KeyCode.Space))
{
    O=true;
}

        Edge1=v[1]-v[0];
        Edge2=v[2]-v[0];
        P=v[0];
        N=(Vector3.Cross(Edge1,Edge2));


     Origin=CamT.position;
     To=CamT.position+(CamT.forward*1000.0f);


Vector3 h=Vector3.Cross(To-Origin,Edge2);


Debug.DrawLine(new Vector3(0,0,0),To-Origin);
Debug.DrawLine(new Vector3(0,0,0),Edge2);

Debug.DrawLine(v[0],h);

float a=Vector3.Dot(Edge1,h);//if zero, ray's parallel

Debug.DrawLine(v[0],v[0]+new Vector3(0,a,0));


float f=1.0f/a;
Vector3 s=Origin-v[0];
float u=f*Vector3.Dot(s,h);

Vector3 q=Vector3.Cross(s,Edge1);
float Q=f*Vector3.Dot(To-Origin,q);

float t=f*Vector3.Dot(Edge2,q);// if above 0, target is in front

T=Origin+(To*t);

Box.transform.position=T;
if(O){
deb+="\n";
deb+="H:";
deb+=h.ToString();


deb+="\n";
deb+="NH:";
deb+=h.normalized.ToString();


deb+="\n";
deb+="A:";
deb+=a.ToString();
deb+="\n";

deb+="F:";
deb+=f.ToString();
deb+="\n";
deb+="U:";
deb+=u.ToString();
deb+="\n";

deb+="S:";
deb+=s.ToString();
deb+="\n";

deb+="Q:";
deb+=q.ToString();
deb+="\n";
deb+="T:";
deb+=t.ToString();
deb+="\n";

deb+="TR:";
deb+=T.ToString();
deb+="\n";

Debug.Log(deb);

}
deb=null;
    }
}

