using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public LookDirection mLookDir;

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        mLookDir = this.GetComponent<LookDirection>();
    }
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        // We only need to take care of look direction. All other logic is handled in the animation, with an animation event
        mLookDir.LookLeft = GameManager.DistanceToPlayerInX(this.transform) < 0;
    }
    /// <summary>
    /// Method called form an animation event, in the proper place to throw the spell
    /// </summary>
    public void SpawnDragonShot()
    {
        GameManager.CurrentLevel.SpawnDragonShot(this.transform.position, this.mLookDir.LookLeft);
    }
}
