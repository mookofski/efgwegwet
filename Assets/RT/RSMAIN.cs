using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
    using UnityEngine;


    public class RSMAIN : MonoBehaviour
    {


        public ComputeShader CPShader;
        private RenderTexture RTraceImage;
      //  public RenderTexture Image;

        
        //*Currently This function Intersepts Raster Render Each Frame to perform RT
        //*this c
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
         
       //     Image=source;
       //     GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),RTraceImage);

        

            Graphics.Blit(RTraceImage, destination);
            
        }
        private void Update() {
          UpdateShaderParam();
            Render();
       //     Image=RTraceImage;

        }



        private void Render()
        {
            // Make sure we have a current render target
            InitRenderTexture();
           
            // Set the target and dispatch the compute shader
            CPShader.SetTexture(0, "Result", RTraceImage);
            int threadGroupsX = Mathf.CeilToInt(Screen.width / 16.0f);
            int threadGroupsY = Mathf.CeilToInt(Screen.height / 16.0f);
            CPShader.Dispatch(0, threadGroupsX, threadGroupsY, 2);
            // Blit the result texture to the screen


            }

        private void InitRenderTexture()
        {
                 if (RTraceImage == null || RTraceImage.width != Screen.width || RTraceImage.height != Screen.height)
        {
            // Release render texture if we already have one
            if (RTraceImage != null)
                RTraceImage.Release();
            // Get a render target for Ray Tracing
            RTraceImage = new RenderTexture(Screen.width, Screen.height, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            RTraceImage.enableRandomWrite = true;
            RTraceImage.Create();
        }
        }
        
    List<Vector4> v3to4(List<Vector3> v)
    {
        List<Vector4> buf=new List<Vector4>();
        foreach(Vector3 b in v)
        {
            buf.Add(new Vector4(b.x,b.y,b.z,0));
        }
        return buf;
    }

    private Camera _camera;

    //* CONSTANT BUFFERS*//
    private ComputeBuffer vtxBuf;
    private int meshcount;


    private List<Vector4> VertexBuf;
    List<int> ObjStride;

    /// <summary>
    /// Vertex Stride, Per Triangle
    /// </summary>
    List<Vector4> TriIndex_ObjStride;   
    //* CONSTANT BUFFER END*//


    //* INSTANCE DATA BUFFER*//
    private List<Transform> Inst_T;
    /// <summary>
    /// *XYZ COLOR, W MODEL INDEX
    /// </summary>
    private List<Vector4> Inst_CI;
    /// <summary>
    ///* X=TYPE Y=INDEX
    /// </summary>
    public List<ObjectType> Inst_TI;
    //* INSTANCE DATA END*//



        
    private void Awake()
    {



        MeshDataSub.RenderData rd=new MeshDataSub.RenderData();
        rd=MeshDataSub.GetVlist();
        
        VertexBuf=v3to4(rd.VertexBuffer);

        ObjStride=new List<int>();



        TriIndex_ObjStride=new List<Vector4>();

        ObjStride.AddRange(rd.ObjectStride);
Debug.Log(ObjStride.Count);
{
        List<int> VertexStride=rd.VerterxStride;

 int k=0;

            for(int i=0;i<VertexStride.Count/3;i++)
        {
                if(ObjStride.Count>i)
                {
                    k=ObjStride[i];
                }

            TriIndex_ObjStride.Add(
                new Vector4(
                    VertexStride[i*3],
                    VertexStride[i*3+1],
                    VertexStride[i*3+2],
                    k/3)
                    );
         
            
//            Debug.Log(TriIndex_ObjStride[i].ToString());
                k=0;
        }
}


        _camera = GetComponent<Camera>();

    
        Inst_T=new List<Transform>();
        Inst_CI = new List<Vector4>();
        Inst_TI= new List<ObjectType>();

        foreach(var b in MeshDataSub.InstanceList)
        {
          Inst_T.Add(b.transform);
          Inst_CI.Add(new Vector4(b.color.r,b.color.g,b.color.b,b.Model));
 
          Inst_TI.Add(b.Type_Index);
  
          Debug.Log(b.Model.ToString());
        }
        Debug.Log("Instance Transform count \n"+Inst_T.Count);





            SetShaderParameters();

    }




public float refrac;
    private void SetShaderParameters()
    {//Name Must Match, case sensitive. but ordering does not
    
        
        CPShader.SetInt("_TriCount",TriIndex_ObjStride.Count);
        CPShader.SetVectorArray("_VertexBuffer",VertexBuf.ToArray());
        CPShader.SetVectorArray("_VertexIndice",TriIndex_ObjStride.ToArray());


    //*Instance count
        CPShader.SetInt("_InstanceCount",MeshDataSub.InstanceList.Count);
    //*Color, Model Index
        CPShader.SetVectorArray("_Inst_ColandIndex",Inst_CI.ToArray());


        CPShader.SetMatrix("_IPJMX", _camera.projectionMatrix.inverse);


 List<Vector4> Col_MIndex=new List<Vector4>();
 foreach(var b in MeshDataSub.InstanceList)
 {
            Col_MIndex.Add(new Vector4(b.color.r,b.color.g,b.color.b,b.Model));
 }
        CPShader.SetVectorArray("_Inst_ColandIndex",Col_MIndex.ToArray());
  


    Debug.Log("VERTEXCOUNT:"+VertexBuf.Count);
    Debug.Log("TRICOUNT"+TriIndex_ObjStride.Count);

    }
    private void UpdateShaderParam()
    {


        List<Vector4> CoordsInst=new List<Vector4>();
        List<Vector4> Type_RIndex=new List<Vector4>();
        List<Matrix4x4> RotInst=new List<Matrix4x4>();  
    

foreach(var b in MeshDataSub.InstanceList)
        {
            CoordsInst.Add(new Vector4(b.transform.position.x,b.transform.position.y,b.transform.position.z,0));
            RotInst.Add(b.transform.localToWorldMatrix);
            Type_RIndex.Add(new Vector4((int)b.Type_Index.Type,b.Type_Index.RefIndex,0,0));
        }
     //*Coords
        CPShader.SetVectorArray("_Inst_Translate",CoordsInst.ToArray());
    //*Scale/Rotation
        CPShader.SetMatrixArray("_Inst_ScaleRot",RotInst.ToArray());
    //*TYPE,REFRAC INDEX


        CPShader.SetVectorArray("_Type_Rindex",Type_RIndex.ToArray());
        CPShader.SetMatrix("_C2WMX", _camera.cameraToWorldMatrix);



    }



    }