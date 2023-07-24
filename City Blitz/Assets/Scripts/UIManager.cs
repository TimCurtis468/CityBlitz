using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text ScoreText;
    public Text LivesText;

    public Sprite[] backgrounds;

    private SpriteRenderer spriteRenderer;

    public GameObject background_obj;
    public GameObject sky_obj;
    public GameObject score;
    public GameObject lives;

    public int Score { get; set; }

    private void Start()
    {
        GameObject obj;
        Transform transform;
        Transform childTransform;

        GameManager.OnLifeGained += OnLifeGained;
        //todo Pooh.OnPoohTargetHit += UpdateScoreText;

        //        GameManager.OnLifeLost += OnLifeLost;
        // add OnTarget Hit subscriptions and call back functions

        transform = background_obj.transform;
        childTransform = transform.Find("Graphics");
        obj = childTransform.gameObject;
        Utilities.ResizeSpriteToFullScreen(obj);

        transform = sky_obj.transform;
        childTransform = transform.Find("Graphics");
        obj = childTransform.gameObject;
        Utilities.ResizeSpriteToFullScreen(obj);

        /*****************************************************************************/
        /* Anchor points in Rect Transform used to anchor text to top left/top right */
        /* So no scaling or positioning needed here for canvas items                 */
        /*****************************************************************************/

        UpdateScoreText(0);
    }

    private void Awake()
    {
        spriteRenderer = background_obj.GetComponent<SpriteRenderer>();

        OnLifeLost(GameManager.Instance.AvailableLives);
    }


    private void OnLifeLost(int remainingLives)
    {

        string txt = "LIVES: " + remainingLives.ToString();
        LivesText.text = txt;


    }

    private void OnLevelComplete()
    {
        if (backgrounds.Length > 0)
        {
            int background_num = UnityEngine.Random.Range(0, backgrounds.Length);
            spriteRenderer.sprite = backgrounds[background_num];
        }
    }

    private void OnLifeGained(int remainingLives)
    {
        string txt = "LIVES: " + remainingLives.ToString();
        LivesText.text = txt;
    }

    public void UpdateScoreText(int increment)
    {
        this.Score += increment;
        string scoreString = this.Score.ToString().PadLeft(5, '0');
        ScoreText.text = "SCORE: " + scoreString;
        GameManager.Instance.SetScore(Score);
    }

    private void OnDisable()
    {
//        GameManager.OnLifeLost -= OnLifeLost;
        GameManager.OnLifeGained -= OnLifeGained;
        //todo Pooh.OnPoohTargetHit -= UpdateScoreText;

    }

}
