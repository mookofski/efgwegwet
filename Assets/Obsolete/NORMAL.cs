using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NORMAL : MonoBehaviour
{
    // Start is called before the first frame update
    public Mesh M;

      private  Vector3[] vs;



        public   List<int> index;
           int I;
     public   List<Vector3> verts;
private Transform T;
    void Start()
    {

        M=GetComponent<MeshFilter>().mesh;
T=this.transform;
    index=new List<int>();
      verts=new List<Vector3>();
 


    }


    Vector3 mult(Vector3 a,Vector3 b)
    {
        return new Vector3(a.x*b.x,a.y*b.y,a.z*b.z);
    }

    // Update is called once per frame
    void Update()
    {
              vs=new Vector3[3];
              I=0;
            M.GetTriangles(index,I);
            M.GetVertices(verts);

            
            for(int i=0;i<index.Count-2;i++)
            {
                vs[0]=T.rotation*mult(T.localScale,verts[index[i]]);
                vs[1]=T.rotation*mult(T.localScale,verts[index[i+1]]);
                vs[2]=T.rotation*mult(T.localScale,verts[index[i+2]]);
            Vector3 temp=((vs[1]-vs[0])/2)+((vs[2]-vs[1])/2)+((vs[0]-vs[2])/2);
          Debug.DrawLine(T.position+temp,T.position+temp+Vector3.Cross(vs[1]-vs[0],vs[2]-vs[0]));

            }

    }

}
