using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LookDirection : MonoBehaviour
{
    private SpriteRenderer mRender;
    private bool mLookLeft;
    
    public bool LookLeft
    {
        get { return mLookLeft; }
        set
        {
            mLookLeft = value;

            // Anytime the LookLeft property changes, make the sprite renderer update the FlipX value
            mRender.flipX = mLookLeft;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        this.mRender = this.GetComponent<SpriteRenderer>();
    }

}
