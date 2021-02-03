using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    /// <summary>
    /// Effector2D that should be disabled to allow the player climb down
    /// </summary>
    public PlatformEffector2D ClimbDownEffector;
    /// <summary>
    /// Main trigger to detect if player is in the ladder
    /// </summary>
    public BoxCollider2D PrincipalTrigger;
    /// <summary>
    /// Trigger on top of ladder, to detect if player is ready to climb down
    /// </summary>
    public BoxCollider2D UpTrigger;
    /// <summary>
    /// Trigger on bottom of ladder, to detect if player is ready to climb up
    /// </summary>
    public BoxCollider2D DownTrigger;

    /// <summary>
    /// 
    /// </summary>
    public void Start()
    {
        // Ensure this ladder is registered in the Ladders collection of the Level        
        // WARNING: This MUST happen AFTER the ladders collection is cleared in the Level, which is done in the AWAKE. So, this must
        //          happen in the Start method.
        if (!GameManager.CurrentLevel.Ladders.Contains(this))
            GameManager.CurrentLevel.Ladders.Add(this);
    }
}
