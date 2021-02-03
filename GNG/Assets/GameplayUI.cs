using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public Text TextScore;
    public Text TextTimeRemaining;
    public Image ImgLife0;
    public Image ImgLife1;
    public Image ImgLife2;

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        // Update UI texts
        this.TextScore.text = GameManager.Player.Score.ToString();
        this.TextTimeRemaining.text = GameManager.CurrentLevel.GetRemainingTimeFormatted();

        // Update Lives Remaining
        this.ImgLife0.enabled = GameManager.Player.Lives >= 1;
        this.ImgLife1.enabled = GameManager.Player.Lives >= 2;
        this.ImgLife2.enabled = GameManager.Player.Lives >= 3;
    }
}
