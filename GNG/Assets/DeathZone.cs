using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    /// <summary>
    /// Death Zones kill the player when he gets inside them. This method will be called each time a physics object gets in the trigger.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered the trigger is the Player
        Player ply = collision.GetComponent<Player>();
        if(ply != null)
            ply.DieArthurDie();
    }
}
