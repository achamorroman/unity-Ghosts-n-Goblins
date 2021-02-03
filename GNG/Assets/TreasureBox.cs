using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    /// <summary>
    /// Type of Pickup hidden in this treasure box
    /// </summary>
    public ePickupType Type = ePickupType.None;

    /// <summary>
    /// 
    /// </summary>
    public void Destroy()
    {
        // Spawn the pickup this treasure box hides, and destroy the treasure box itself
        GameManager.CurrentLevel.SpawnPickUp(this.Type, this.transform.position);
        GameObject.Destroy(this.gameObject);
    }
}
