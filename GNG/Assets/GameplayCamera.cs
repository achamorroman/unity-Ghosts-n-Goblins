using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCamera : MonoBehaviour
{
    public float MinCameraX;
    public float MaxCameraX;
    private Vector3 mInitialPos;

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        mInitialPos = this.transform.position;
    }
    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        this.transform.position = mInitialPos;
    }
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        // Make the camera follow the player
        float newX = GameManager.Player.transform.position.x + 4;
        newX = Mathf.Max(newX, MinCameraX);
        newX = Mathf.Min(newX, MaxCameraX);
        this.transform.position = new Vector3(newX, this.transform.position.y, this.transform.position.z);
    }
}
