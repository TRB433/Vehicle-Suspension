using UnityEngine;

public class WheelType : MonoBehaviour
{

    public enum WheelPosition{LeftFront, RightFront, LeftRear, RightRear}
    public WheelPosition wheelType;

    [HideInInspector]
    public Vector2 LeftFrontOffset = new Vector2(-0.75f, 0.4f);
    [HideInInspector]
    public Vector2 RightFrontOffset = new Vector2(0.75f, 0.4f);
    [HideInInspector]
    public Vector2 LeftRearOffset = new Vector2(-0.75f, -0.4f);
    [HideInInspector]
    public Vector2 RightRearOffset = new Vector2(0.75f, -0.4f);
    
}
