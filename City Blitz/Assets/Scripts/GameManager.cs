using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum eEndOfLavelState
{
    eIdle,
    eEndOfLevelDelay
}

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance => _instance;


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public int AvailableLives = 3;
    public int Lives { get; set; } = 3;

    public bool IsGameStarted { get; set; }
        public bool paused = false;

//    public static event Action<int> OnLifeLost;
    public static event Action<int> OnLifeGained;
    public static event Action<int> OnLevelComplete;

    //    public GameObject background;
    public GameObject gameOver;
    public bool buffActive = false;
    public bool gameOverActive = false;


    private int endScore = 0;

    private int level = 1;
    ////    private int nextAd = 2;

    //    private int numResurrections = 0;

    /* End of level variables */
    private eEndOfLavelState endOfLevelStateMachine = eEndOfLavelState.eIdle;
    private Stopwatch sw = new Stopwatch();
    private int endOfLevelTimeout = 1000;

    private void Start()
    {

        if (gameOver != null)
        {
            gameOver.SetActive(false);
        }

        this.Lives = AvailableLives;
        //        Ball.OnPoohHit += OnPoohHit;

        //       AdManager.Instance.RequestBanner(GoogleMobileAds.Api.AdPosition.Top);
        //        AdManager.Instance.RequestRewarded();

        paused = false;

        level = 1;
//        nextAd = 4;
//        numResurrections = 0;
        buffActive = false;
        gameOverActive = false;

        Utilities.ResizeSprite(this.gameObject);
        //todo TargetManager.Instance.CreateTarget();
        //todo TargetManager.Instance.CreateTarget();
        //todo TargetManager.Instance.CreateTarget();

        //todo CloudManager.Instance.CreateStartClouds();

    }

    public void Update()
    {

        /* Todo - Put into Game Manager */
        bool all_destroyed = BuildingBlockManager.Instance.AllBlockDestroyed();

        if (gameOverActive == false)
        {
            if (all_destroyed == true)
            {
                if (endOfLevelStateMachine == eEndOfLavelState.eEndOfLevelDelay)
                {
                    /* Wait for timeout */
                    if (sw.ElapsedMilliseconds > endOfLevelTimeout)
                    {
                        level++;
                        OnLevelComplete?.Invoke(this.level);
                        BuildingBlockManager.Instance.GenerateBlocks(level);
                        sw.Reset();
                        endOfLevelStateMachine = eEndOfLavelState.eIdle;
                        Plane.Instance.resetPlane();
                    }
                }
                else
                {
                    sw.Start();        /* Check for end of level */
                    endOfLevelStateMachine = eEndOfLavelState.eEndOfLevelDelay;
                }
            }
        }
#if PI
        else
        {
            if (AdManager.Instance.rewardedAdClosed == true)
            {
                if ((AdManager.Instance.rewardGiven == true) && (AdManager.Instance.rewardedAdFailed == false))
                {
                    this.GameOverExtraLife();
                }
                else
                {
                    this.MoveToGameOver();
                }
                AdManager.Instance.rewardGiven = false;
                AdManager.Instance.rewardedAdClosed = false;
                gameOverActive = false;
            }
            else if (AdManager.Instance.rewardedAdFailed == true)
            {
                AdManager.Instance.rewardedAdFailed = false;
                this.MoveToGameOver();
                gameOverActive = false;
            }
        }
#endif
    }

    public void SetScore(int score)
    {
        endScore = score;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        this.Lives = AvailableLives;
        level = 1;
//        nextAd = 4;
    }

    public void AddLife()
    {
//        SoundFxManager.Instance.PlayHeart();
        Lives++;
        OnLifeGained?.Invoke(this.Lives);
    }

    public void DecrementLives()
    {
        Lives--;

        if(Lives <= 0)
        {
            /* GAME OVER */
            /* Show buttons to select resurrection or death */
            if (gameOver != null)
            {
                gameOver.SetActive(true);
                gameOverActive = true;
                Plane.Instance.game_over = true;
                //MusicManager.Instance.StopMusic();
            }
        }
    }
#if (PI)
    private void DeathCheck()
    {
        if (BallsManager.Instance.Balls.Count <= 0)
        {
            this.Lives--;

            if (this.Lives < 1)
            {
                BallsManager.Instance.DestroyBalls();
                /* Check how many times player has been resurrected and die forever if too many */
                if (numResurrections < 2)
                {
                    /* Show buttons to select resurrection or death */
                    if (gameOver != null)
                    {
                        gameOver.SetActive(true);
                        gameOverActive = true;
                        Paddle.Instance.isActive = false;
                        Paddle.Instance.PaddleIsShooting = false;
                        MusicManager.Instance.StopMusic();
                    }
                }
                else
                {
                    MoveToGameOver();
                }
            }
            else
            {
                OnLifeLost?.Invoke(this.Lives);
                BallsManager.Instance.ResetBalls();
                IsGameStarted = false;
            }
        }
    }

    public void GameOverWatchVid()
    {
        AdManager.Instance.ShowRewardedAd();
    }

    public void GameOverExtraLife()
    {
        this.Lives++;
        nextAd += 2;
        numResurrections++;
        gameOver.SetActive(false);
        AdManager.Instance.DestroyRewarded();

        OnLifeLost?.Invoke(this.Lives);
        BallsManager.Instance.ResetBalls();
        IsGameStarted = false;
        Paddle.Instance.isActive = true;
        MusicManager.Instance.StartMusic();
        AdManager.Instance.RequestRewarded();

    }
#endif

    public void MoveToGameOver()
    {
        gameOver.SetActive(false);
//        AdManager.Instance.DestroyRewarded();
//        AdManager.Instance.DestroyBanner();

//        Paddle.Instance.isActive = true;
//        EndScreen.score = endScore;
        SceneManager.LoadScene("GameOver");
    }
#if (PI)
    private void OnDisable()
    {
        Ball.OnBallDeath -= OnBallDeath;
        Brick.OnBrickDistruction -= OnBrickDestruction;
    }

    private void OnBrickDestruction(Brick obj)
    {
//        if (BricksManager.Instance.RemainingBricks.Count <= 0)
        {
            paused = true;
            if (level == nextAd)
            {
 //               AdManager.Instance.RequestInterstital();
                int rand = UnityEngine.Random.Range(3, 5);
                nextAd += rand;
            }

            // Pause for 1 second 
            StartPause(1);
        }
    }
#endif
    public void StartPause(float pause)
    {
        // how many seconds to pause the game
        StartCoroutine(PauseGame(pause));
    }
    public IEnumerator PauseGame(float pauseTime)
    {
        float pauseEndTime = Time.realtimeSinceStartup + pauseTime;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }

        PauseEnded();
    }

    public void PauseEnded()
    {
        paused = false;
        level++;

        GameManager.Instance.IsGameStarted = false;

    }
}


