using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text ScoreText;
    public Text LivesText;
    public Text LevelsText;

    public Sprite[] day_backgrounds;
    public Sprite[] night_backgrounds;

    private SpriteRenderer spriteRenderer;

    public GameObject background_obj;

    private bool is_daytime = true;
    
    public int Score { get; set; }

    private void Start()
    {
        //GameObject obj;
        //Transform transform;
        //Transform childTransform;

        GameManager.OnLifeGained += OnLifeGained;

        GameManager.OnLevelComplete += OnLevelComplete;
        BuildingBlock.OnPlaneCrash += OnPlaneCrash;
        Bomb.OnBombTargetHit += UpdateScoreText;
        UpdateScoreText(0);

        //transform = background_obj.transform;
        //childTransform = transform.Find("Graphics");
        //obj = childTransform.gameObject;
        //Utilities.ResizeSpriteToFullScreen(obj);

    }

    private void Awake()
    {
        is_daytime = true;
        spriteRenderer = background_obj.GetComponent<SpriteRenderer>();

        string txt = "LIVES: " + GameManager.Instance.Lives.ToString();
        LivesText.text = txt;
    }


    private void OnPlaneCrash()
    {
        GameManager.Instance.DecrementLives();
        string txt = "LIVES: " + GameManager.Instance.Lives.ToString();
        LivesText.text = txt;
    }

    private void OnLevelComplete(int newLevel)
    {
        string txt = "Level: " + newLevel.ToString();
        LevelsText.text = txt;

        if (is_daytime == true)
        {
            if (day_backgrounds.Length > 0)
            {
                int background_num = UnityEngine.Random.Range(0, day_backgrounds.Length);
                spriteRenderer.sprite = day_backgrounds[background_num];
            }
            is_daytime = false;
        }
        else
        {
            if (night_backgrounds.Length > 0)
            {
                int background_num = UnityEngine.Random.Range(0, night_backgrounds.Length);
                spriteRenderer.sprite = night_backgrounds[background_num];
            }

            is_daytime = true;

        }
    }

    private void OnLifeGained(int remainingLives)
    {
        string txt = "LIVES: " + remainingLives.ToString();
        LivesText.text = txt;
    }
#if (PI)
    private void OnPartDistruction(BuildingPart obj)
    {
        UpdateScoreText(10);
    }

    private void OnPartHit(BuildingPart obj, int increment)
    {
        UpdateScoreText(increment);
    }
#endif

    private void UpdateScoreText(int increment)
    {
        this.Score += increment;
        string scoreString = this.Score.ToString().PadLeft(5, '0');
        ScoreText.text = "SCORE: " + scoreString;
        GameManager.Instance.SetScore(Score);
    }



    private void OnDisable()
    {
        GameManager.OnLevelComplete -= OnLevelComplete;
        BuildingBlock.OnPlaneCrash -= OnPlaneCrash;
        Bomb.OnBombTargetHit -= UpdateScoreText;
        GameManager.OnLifeGained -= OnLifeGained;

    }

}
