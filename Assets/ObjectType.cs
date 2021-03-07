using UnityEngine;

public class ObjectType : MonoBehaviour
{
public enum ObjectTypeID
{opaque=0,reflective=2,refractive=1};

public ObjectTypeID Type;

public float RefIndex=1;


}
