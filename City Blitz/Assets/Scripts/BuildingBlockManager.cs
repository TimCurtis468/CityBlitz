using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;


enum eEndOfLavelState
{
    eIdle,
    eEndOfLevelDelay
}


public class BuildingBlockManager : MonoBehaviour
{
    #region Singleton
    private static BuildingBlockManager _instance;
    public static BuildingBlockManager Instance => _instance;

    public static event System.Action OnLevelLoaded;

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

    private int MAX_FLOORS = 2;//8;
    private int numBuildings = 2;//15;
    private float initialBlockSpawnPositionX = -2.075f;
    private float initialBlockSpawnPositionY = -4.5f;
    private float xshiftAmount = 0.275f;
    private float yshiftAmount = 0.725f;

    private int[] building_sizes = new int[15];

    private GameObject bricksContainer;

    //public static event Action OnLevelComplete;



    public BuildingBlock blockPrefab;

    public Sprite[] baseSprites;
    public Sprite[] midSprites;
    public Sprite[] topSprites;

    public List<BuildingBlock> RemainingBlocks { get; set; }

    public int levelNum = 0;

    public int InitialBlocksCount { get; set; }

    /* End of level variables */
    private eEndOfLavelState endOfLevelStateMachine = eEndOfLavelState.eIdle;
    Stopwatch sw = new Stopwatch();


    // Start is called before the first frame update
    void Start()
    {
        this.bricksContainer = new GameObject("BlocksContainer");
        this.GenerateBlocks();
        levelNum = 0;

    }

    private void Update()
    {

        /* Todo - Put into Game Manager */
        bool all_destroyed = AllBlockDestroyed();

        if(all_destroyed == true)
        {
            if(endOfLevelStateMachine == eEndOfLavelState.eEndOfLevelDelay)
            {
                /* Wait for timeout */
                if(sw.ElapsedMilliseconds > 5000)
                {
                    levelNum++;
                    GenerateBlocks();
                    sw.Reset();
                    endOfLevelStateMachine = eEndOfLavelState.eIdle;
                }
            }
            else
            {
                sw.Start();        /* Check for end of level */
                endOfLevelStateMachine = eEndOfLavelState.eEndOfLevelDelay;
            }
        }
    }

    public bool AllBlockDestroyed()
    {
        bool all_destroyed = true;
        int block_count = 0;

        for (int idx = 0; idx < this.RemainingBlocks.Count; idx++)
        {
            if(RemainingBlocks[idx] != null)
            {
                all_destroyed = false;
                block_count++;
            }
        }

       return all_destroyed;
    }

    private void GenerateBlocks()
    {
        this.RemainingBlocks = new List<BuildingBlock>();
        float currentSpawnX = Utilities.ResizeXValue(initialBlockSpawnPositionX);
        float currentSpawnY = Utilities.ResizeYValue(initialBlockSpawnPositionY);
        float zShift = 0;

        int row = 0;
        int building_image_num = UnityEngine.Random.Range(1, this.baseSprites.Length);
        int size = 0;
        int b_color = 0xFFFFFF;

        BuildingBlock newBlock;

        /* Base row */
        for (int building_num = 0; building_num < this.numBuildings; building_num++)
        {
            building_image_num = UnityEngine.Random.Range(1, this.baseSprites.Length);
            size = UnityEngine.Random.Range(1, MAX_FLOORS + 1);
            building_sizes[building_num] = size;

            b_color = UnityEngine.Random.Range(0x7F, 0xFF);
            b_color = (b_color << 8) + UnityEngine.Random.Range(0x7F, 0xFF);
            b_color = (b_color << 8) + UnityEngine.Random.Range(0x7F, 0xFF);

            for (row = 0; row <= size; row++)
            {
                if (row == 0)
                {
                    newBlock = Instantiate(blockPrefab, new Vector3(currentSpawnX, currentSpawnY, 0 - zShift), Quaternion.identity) as BuildingBlock;
                    newBlock.Init(bricksContainer.transform, this.baseSprites[building_image_num], GetColour(b_color), 1);
                }
                else if (row < size)
                {
                    newBlock = Instantiate(blockPrefab, new Vector3(currentSpawnX, currentSpawnY, 0 - zShift), Quaternion.identity) as BuildingBlock;
                    newBlock.Init(bricksContainer.transform, this.midSprites[building_image_num], GetColour(b_color), 1);
                }
                else
                {
                    newBlock = Instantiate(blockPrefab, new Vector3(currentSpawnX, currentSpawnY, 0 - zShift), Quaternion.identity) as BuildingBlock;
                    newBlock.Init(bricksContainer.transform, this.topSprites[building_image_num], GetColour(b_color), 1);
                }

                this.RemainingBlocks.Add(newBlock);
                zShift += 0.0001f;

                currentSpawnY += Utilities.ResizeYValue(yshiftAmount);
                zShift += 0.0001f;
            }
            zShift = ((building_num + 1) * 0.0005f);

            currentSpawnX += Utilities.ResizeXValue(xshiftAmount);
            currentSpawnY = Utilities.ResizeYValue(initialBlockSpawnPositionY);

        }

        this.InitialBlocksCount = this.RemainingBlocks.Count;
        OnLevelLoaded?.Invoke();

    }

    private void ClearRemainingBlocks()
    {
        foreach (BuildingBlock block in this.RemainingBlocks.ToList())
        {
            Destroy(block.gameObject);
        }
    }

    private Color GetColour(int intColour)
    {
        int r = (intColour >> 16) & 0xFF;
        int g = (intColour >> 8) & 0xFF;
        int b = intColour & 0xFF;

        Color color = new Color(r / 255.0f, g / 255.0f, b / 255.0f);

        return color;
    }

}
