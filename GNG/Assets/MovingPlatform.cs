using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    /// <summary>
    /// Trigger used to detect if the player is inside
    /// </summary>
    public BoxCollider2D TriggerDetector;
    /// <summary>
    /// Horizontal movement, in m/s
    /// </summary>
    public float SpeedX;
    /// <summary>
    /// Allowed range of movement
    /// </summary>
    public float RangeM = 5;

    [HideInInspector]
    public Rigidbody2D mRigidBody;
    private int Direction = 1;
    private float mInitialPosX;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        mRigidBody = this.GetComponent<Rigidbody2D>();
    }
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        mInitialPosX = this.transform.position.x;

        // Ensure this platform is registered in the MovingPlatforms collection of the Level        
        // WARNING: This MUST happen AFTER the MovingPlatforms collection is cleared in the Level, which is done in the AWAKE. So, this must
        //          happen in the Start method.
        if (!GameManager.CurrentLevel.MovingPlatforms.Contains(this))
            GameManager.CurrentLevel.MovingPlatforms.Add(this);
    }
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        mRigidBody.velocity = new Vector2(Direction * SpeedX, 0);

        // Check if out of range to make the platform turn back
        float dif = this.transform.position.x - mInitialPosX;
        if (dif > RangeM)
            Direction = -1;
        if (dif < -RangeM)
            Direction = 1;
    }    
    

}
