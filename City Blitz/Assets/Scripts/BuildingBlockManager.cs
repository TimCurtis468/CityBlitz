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

    //private int numRows = 12;
    public int numCols = 10;
    public float initialBrickSpawnPositionX = -2.00f;
    public float initialBrickSpawnPositionY = -4.0f;
    public float xshiftAmount = 0.4385f;
    public float yshiftAmount = 0.42f;

    private GameObject bricksContainer;

    //public static event Action OnLevelComplete;


    public BuildingBlock blockPrefab;

    public Sprite[] Sprites;

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
        float currentSpawnX = Utilities.ResizeXValue(initialBrickSpawnPositionX);
        float currentSpawnY = Utilities.ResizeYValue(initialBrickSpawnPositionY);
        float zShift = 0;

        int row = 0;
        //for (int row = 0; row < this.numRows; row++)
        //{
            for (int col = 0; col < this.numCols; col++)
            {
                    BuildingBlock newBlock = Instantiate(blockPrefab, new Vector3(currentSpawnX, currentSpawnY, 0 - zShift), Quaternion.identity) as BuildingBlock;
                    newBlock.Init(bricksContainer.transform, this.Sprites[0], GetColour(0xFFFFFF), 1);

                    //Utilities.ResizeSprite(newBlock.gameObject);

                    this.RemainingBlocks.Add(newBlock);
                    zShift += 0.0001f;

                currentSpawnX += Utilities.ResizeXValue(xshiftAmount);
                if (col + 1 >= this.numCols)
                {
                    currentSpawnX = Utilities.ResizeXValue(initialBrickSpawnPositionX);
                }
            }

            currentSpawnY -= Utilities.ResizeYValue(yshiftAmount);
            zShift = ((row + 1) * 0.0005f);
        //}

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
