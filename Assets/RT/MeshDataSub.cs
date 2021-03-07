using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshDataSub : MonoBehaviour
{

    public static List<Mesh> Mlist;
    public List<string> Midentity;
    public static int MIndex;
/*
    struct transform
    {

        public Vector3 pos;
        public Vector3 scale;
        public Vector3 rot;
    };
    private transform maketransform(Vector3 pos, Vector3 sc, Vector3 rot)
    {
        transform buf = new transform();
        buf.pos = pos;
        buf.scale = sc;
        buf.rot = rot;
        return buf;
    }*/

    public struct ObjectInstnce
    {
        public Transform transform;
        public int Model;
        public Color color;
        /// <summary>
        /// X:Type Y:ModelID
        /// </summary>
        public ObjectType Type_Index;
    }

    public ObjectInstnce MakeOinstnce(Transform t,int modelindex,Color col,ObjectType ot)
    {
        ObjectInstnce buf=new ObjectInstnce();
        buf.transform=t;
        buf.Model=modelindex;
        buf.color=col;
        buf.Type_Index=ot;
        return buf;
    }


    public struct RenderData
    {
        public List<Vector3> VertexBuffer;


        /// <summary>
        ///stores vertex index: Made per Tri
        /// </summary>
        public List<int> VerterxStride;


        /// <summary>
        ///store position of each object's vertex head: Made per Mesh//
        /// </summary>
        public int[] ObjectStride;

    };
public static bool constructed=false;

public static List<ObjectInstnce> InstanceList;
    private void Awake() 
    {
        InstanceList=new List<ObjectInstnce>();
        Mlist = new List<Mesh>();
        List<MeshFilter> mlist_temp = new List<MeshFilter>();



        mlist_temp.AddRange(FindObjectsOfType<MeshFilter>());

 

        bool dupe = false;

        
        Midentity=new List<string>();

        //initialize first index
        foreach (MeshFilter b in mlist_temp)
        {
            dupe = false;
            
            foreach (Mesh c in Mlist)
            {
                if (Equals(c.name, b.mesh.name))
                {
                    dupe = true;
                    break;
                }
            }
            if (!dupe)
            {
                Mlist.Add(b.mesh);
                Midentity.Add(b.mesh.name);
            }
    

        b.TryGetComponent(out ObjectType ot);
    if(ot==null)
    {
        ot=new ObjectType();
        ot.Type=0;
        ot.RefIndex=0;
    }
        InstanceList.Add(
            MakeOinstnce(
                b.GetComponentInParent<Transform>(),
                Midentity.IndexOf(b.mesh.name),
                b.GetComponentInParent<MeshRenderer>().material.color,
                ot
            ));



        }
        MIndex = Mlist.Count;
        constructed=true;


    }

/// <summary>
/// Creates Constant Data to be sent to the Shader| |
/// Vertex Coords, Indices, Index for index etc
/// </summary>
/// <returns></returns>
public   static RenderData GetVlist()
    {

        RenderData rb = new RenderData();
        rb.VertexBuffer=new List<Vector3>();
        rb.VerterxStride=new List<int>();
        rb.ObjectStride=new int[Mlist.Count*3];
{


        int k=0;
        //*Offsets index value 
        int IndexOffset=0;
            for (int i = 0; i < Mlist.Count; i++)//*RUN PER UNIQUE MODEL
            {

                //vertex data
                rb.VertexBuffer.AddRange(Mlist[i].vertices);
                //triangle index
                rb.VerterxStride.AddRange(Mlist[i].triangles);
                //triangle index offset per unique model

                rb.ObjectStride[i*3]=k;//start
                rb.ObjectStride[(i*3)+1]=rb.VerterxStride.Count-k;//count

                //Hightest value of each index per model    
                rb.ObjectStride[(i*3)+2]=IndexOffset*3;

            //*update cumilative index offset  
                IndexOffset+=(Mlist[i].triangles.Max()+1);


                k=rb.VerterxStride.Count;

            }
}

        return rb;

    }



    // Update is called once per frame
}
