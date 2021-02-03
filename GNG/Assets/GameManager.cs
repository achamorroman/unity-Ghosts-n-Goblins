using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityEngine;

public static class GameManager
{
    public static Player Player;
    public static Level CurrentLevel;

    /// <summary>
    /// Returns the horizontal distance to the Player, in m
    /// </summary>
    /// <param name="pTransform"></param>
    /// <returns></returns>
    public static float DistanceToPlayerInX(Transform pTransform)
    {
        return Player.transform.position.x - pTransform.position.x;
    }
    /// <summary>
    /// Tool to allow choosing true/false randomly, like when flipping a coin
    /// </summary>
    /// <returns></returns>
    public static bool FlipCoin()
    {
        return Random.Range(0f, 1f) > 0.5f;
    }
    /// <summary>
    /// Resets the current level and player to its initial state (called when the player dies)
    /// </summary>
    public static void ResetLevel()
    {
        Player.ResetPlayer();
        CurrentLevel.ResetLevel();
    }
    /// <summary>
    /// Called when the current level is finished successfully
    /// </summary>
    public static void CurrentLevelFinished()
    { 
        // Intentionally left blank
    }
}

