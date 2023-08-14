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

    private int numFloors = 6;
    private int numBuildings = 15;
    private float initialBlockSpawnPositionX = -2.075f;
    private float initialBlockSpawnPositionY = -4.5f;
    private float xshiftAmount = 0.275f;
    private float yshiftAmount = 0.725f;

    private GameObject bricksContainer;

    //public static event Action OnLevelComplete;


    public BuildingBlock blockPrefab;

    public Sprite[] baseSprites;
    public Sprite[] midSprites;
    public Sprite[] topSprites;

    public List<BuildingBlock> RemainingBlocks { get; set; }

    public int levelNum = 0;

    public int InitialBlocksCount { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        this.bricksContainer = new GameObject("BlocksContainer");
        this.GenerateBlocks();
        levelNum = 0;

    }

    private void GenerateBlocks()
    {
        this.RemainingBlocks = new List<BuildingBlock>();
        float currentSpawnX = Utilities.ResizeXValue(initialBlockSpawnPositionX);
        float currentSpawnY = Utilities.ResizeYValue(initialBlockSpawnPositionY);
        float zShift = 0;

        int row = 0;
        int building_num = UnityEngine.Random.Range(1, 3);

        BuildingBlock newBlock;

        /* Base row */
        for (int col = 0; col < this.numBuildings; col++)
        {
            building_num = UnityEngine.Random.Range(1, 3);

            for (row = 0; row <= this.numFloors; row++)
            {
                if (row == 0)
                {
                    newBlock = Instantiate(blockPrefab, new Vector3(currentSpawnX, currentSpawnY, 0 - zShift), Quaternion.identity) as BuildingBlock;
                    newBlock.Init(bricksContainer.transform, this.baseSprites[building_num], GetColour(0xFFFFFF), 1);
                }
                else if (row < this.numFloors)
                {
                    newBlock = Instantiate(blockPrefab, new Vector3(currentSpawnX, currentSpawnY, 0 - zShift), Quaternion.identity) as BuildingBlock;
                    newBlock.Init(bricksContainer.transform, this.midSprites[building_num], GetColour(0xFFFFFF), 1);
                }
                else
                {
                    newBlock = Instantiate(blockPrefab, new Vector3(currentSpawnX, currentSpawnY, 0 - zShift), Quaternion.identity) as BuildingBlock;
                    newBlock.Init(bricksContainer.transform, this.topSprites[building_num], GetColour(0xFFFFFF), 1);
                }

                this.RemainingBlocks.Add(newBlock);
                zShift += 0.0001f;

                currentSpawnY += Utilities.ResizeYValue(yshiftAmount);
                zShift += 0.0001f;
            }
            zShift = ((col + 1) * 0.0005f);

            currentSpawnX += Utilities.ResizeXValue(xshiftAmount);
            currentSpawnY = Utilities.ResizeYValue(initialBlockSpawnPositionY);

        }

#if PI
        newBlock = Instantiate(blockPrefab, new Vector3(currentSpawnX, currentSpawnY, 0 - zShift), Quaternion.identity) as BuildingBlock;
            newBlock.Init(bricksContainer.transform, this.baseSprites[1], GetColour(0xFFFFFF), 1);

            //Utilities.ResizeSprite(newBlock.gameObject);

            this.RemainingBlocks.Add(newBlock);
            zShift += 0.0001f;

            currentSpawnX += Utilities.ResizeXValue(xshiftAmount);
            if (col + 1 >= this.numBuildings)
            {
                currentSpawnX = Utilities.ResizeXValue(initialBlockSpawnPositionX);
            }
        }

        currentSpawnY += Utilities.ResizeYValue(yshiftAmount);
        zShift = ((row + 1) * 0.0005f);


        for (row = 1; row <= this.numFloors; row++)
        {

            for (int col = 0; col < this.numBuildings; col++)
            {
                BuildingBlock newBlock = Instantiate(blockPrefab, new Vector3(currentSpawnX, currentSpawnY, 0 - zShift), Quaternion.identity) as BuildingBlock;
                newBlock.Init(bricksContainer.transform, this.midSprites[1], GetColour(0xFFFFFF), 1);

                //Utilities.ResizeSprite(newBlock.gameObject);

                this.RemainingBlocks.Add(newBlock);
                zShift += 0.0001f;

                currentSpawnX += Utilities.ResizeXValue(xshiftAmount);
                if (col + 1 >= this.numBuildings)
                {
                    currentSpawnX = Utilities.ResizeXValue(initialBlockSpawnPositionX);
                }
            }

            currentSpawnY += Utilities.ResizeYValue(yshiftAmount);
            zShift = ((row + 1) * 0.0005f);
        }

        row = this.numFloors + 1;

        for (int col = 0; col < this.numBuildings; col++)
        {
            BuildingBlock newBlock = Instantiate(blockPrefab, new Vector3(currentSpawnX, currentSpawnY, 0 - zShift), Quaternion.identity) as BuildingBlock;
            newBlock.Init(bricksContainer.transform, this.topSprites[1], GetColour(0xFFFFFF), 1);

            //Utilities.ResizeSprite(newBlock.gameObject);

            this.RemainingBlocks.Add(newBlock);
            zShift += 0.0001f;

            currentSpawnX += Utilities.ResizeXValue(xshiftAmount);
            if (col + 1 >= this.numBuildings)
            {
                currentSpawnX = Utilities.ResizeXValue(initialBlockSpawnPositionX);
            }
        }
#endif
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
