using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



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

    private int MAX_FLOORS = 8;
    private int MAX_NUM_BUILDINGS = 15;
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

    public int InitialBlocksCount { get; set; }

    /* Define {num buldings, num blocks} for each level  */
    private int[,] levels = {
        { 0, 0 },
        { 2, 3 },
        { 3, 3 },
        { 3, 4 },
        { 4, 4 },
        { 5, 5 },
        { 6, 5 },
        { 7, 6 },
        { 8, 6 },
        { 10, 7 },
        { 11, 7 },
        { 12, 8 },
        { 13, 8 },
        { 14, 8 },
        { 15, 8 }
    };


    // Start is called before the first frame update
    void Start()
    {
        this.bricksContainer = new GameObject("BlocksContainer");
        this.GenerateBlocks(1);
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

    public void GenerateBlocks(int level)
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

        int num_buildings = 2;
        int num_floors = 3;

        if(level < levels.Length)
        {
            num_buildings = levels[level, 0];
            num_floors = levels[level, 1];
        }
        else
        {
            num_buildings = MAX_NUM_BUILDINGS;
            num_floors = MAX_FLOORS;
        }

        for (int building_num = 0; building_num < num_buildings; building_num++)
        {
            int building_idx = UnityEngine.Random.Range(0, MAX_NUM_BUILDINGS);

            building_image_num = UnityEngine.Random.Range(1, this.baseSprites.Length);
            size = UnityEngine.Random.Range(2, num_floors);

            b_color = UnityEngine.Random.Range(0x7F, 0xFF);
            b_color = (b_color << 8) + UnityEngine.Random.Range(0x7F, 0xFF);
            b_color = (b_color << 8) + UnityEngine.Random.Range(0x7F, 0xFF);

            if (building_sizes[building_idx] == 0)
            {
                building_sizes[building_idx] = size;
            }
            else
            {
                int count = 0;
                /* Search for next free slot */
                while ((building_sizes[building_idx] != 0) && (count < MAX_NUM_BUILDINGS))
                {
                    building_idx++;
                    /* Move to start of array if end is reached */
                    if (building_idx >= MAX_NUM_BUILDINGS)
                    {
                        building_idx = 0;
                    }
                    Debug.Log("building_idx: " + building_idx.ToString());
                    /* Is this an empty slot? */
                    if (building_sizes[building_idx] == 0)
                    {
                        /* Yes - store size */
                        building_sizes[building_idx] = size;
                        break;
                    }
                    count++;
                }
            }

            /* Create building */
            currentSpawnX = Utilities.ResizeXValue(initialBlockSpawnPositionX);
            currentSpawnX += Utilities.ResizeXValue(xshiftAmount * building_idx);
            Debug.Log("currentSpawnX: " + currentSpawnX.ToString());
            currentSpawnY = Utilities.ResizeYValue(initialBlockSpawnPositionY);

            /* Add rows of blocks */
            for (row = 1; row <= size; row++)
            {
                if (row == 1)
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
        }

        this.InitialBlocksCount = this.RemainingBlocks.Count;
        OnLevelLoaded?.Invoke();



#if PI
        /* Base row */
        for (int building_num = 0; building_num < num_buildings; building_num++)
        {
            building_image_num = UnityEngine.Random.Range(1, this.baseSprites.Length);
            size = UnityEngine.Random.Range(2, num_floors);
            building_sizes[building_num] = size;

            b_color = UnityEngine.Random.Range(0x7F, 0xFF);
            b_color = (b_color << 8) + UnityEngine.Random.Range(0x7F, 0xFF);
            b_color = (b_color << 8) + UnityEngine.Random.Range(0x7F, 0xFF);

            for (row = 1; row <= size; row++)
            {
                if (row == 1)
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
#endif
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
