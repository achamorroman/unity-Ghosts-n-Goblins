using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public ePickupType Type = ePickupType.None;

    private Animator mAnimator;

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        mAnimator = this.GetComponent<Animator>();
    }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        // Update animation on startup, basedon pickup type
        switch (this.Type)
        {
            case ePickupType.Armor:
                this.mAnimator.Play("PrizeArmor");
                break;
            case ePickupType.Axe:
                this.mAnimator.Play("PrizeAxe");
                break;
            case ePickupType.Cross:
                this.mAnimator.Play("PrizeCross");
                break;
            case ePickupType.Dagger:
                this.mAnimator.Play("PrizeDagger");
                break;
            case ePickupType.Torch:
                this.mAnimator.Play("PrizeFire");
                break;
            case ePickupType.Shield:
                this.mAnimator.Play("PrizeShield");
                break;
            case ePickupType.Spear:
                this.mAnimator.Play("PrizeSpear");
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player ply = collision.gameObject.GetComponent<Player>();
        if (ply != null)
            ply.Pickup(this);

        GameObject.Destroy(this.gameObject);
    }
}
