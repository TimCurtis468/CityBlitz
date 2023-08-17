using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text ScoreText;
    public Text LivesText;

    public Sprite[] day_backgrounds;

    private SpriteRenderer spriteRenderer;

    public GameObject background_obj;

    public int Score { get; set; }

    private void Start()
    {
        //GameObject obj;
        //Transform transform;
        //Transform childTransform;

        GameManager.OnLifeGained += OnLifeGained;
        //todo GameManager.OnLifeLost += OnLifeLost;
        //todo BuldingManager.OnLevelComplete += OnLevelComplete;
        //todo BuildingPart.OnPartDistruction += OnPartDistruction;
        Bomb.OnBombTargetHit += UpdateScoreText;
        UpdateScoreText(0);

        //transform = background_obj.transform;
        //childTransform = transform.Find("Graphics");
        //obj = childTransform.gameObject;
        //Utilities.ResizeSpriteToFullScreen(obj);

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
        if (day_backgrounds.Length > 0)
        {
            int background_num = UnityEngine.Random.Range(0, day_backgrounds.Length);
            spriteRenderer.sprite = day_backgrounds[background_num];
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
        //GameManager.OnLifeLost -= OnLifeLost;
        //BuldingManager.OnLevelComplete -= OnLevelComplete;
        //BuildingPart.OnPartDistruction -= OnPartDistruction;
        Bomb.OnBombTargetHit -= UpdateScoreText;

        GameManager.OnLifeGained -= OnLifeGained;

    }

}
