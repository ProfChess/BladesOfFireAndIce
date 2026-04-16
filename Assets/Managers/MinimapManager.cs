using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    private HashSet<Vector2Int> FloorPositions; //Positions of Floor Tiles
    private const int viewRadius = 8;           //Distance Shown in Map 
    private Transform playerTransform;
    private Texture2D mapTexture;
    [SerializeField] private RawImage miniMapImage;

    //Updating Map
    private float MapUpdateInterval = 0.1f;
    private float MapUpdateTimer = 0f;

    private void OnEnable()
    {
        DungeonCreationV2.DungeonDataCreated += AssignFloor;
    }
    private void OnDisable()
    {
        DungeonCreationV2.DungeonDataCreated -= AssignFloor;
    }

    public void AssignFloor()
    {
        //Assign Ref
        if (GameManager.Instance == null || GameManager.Instance.getPlayer() == null) { return; }
        playerTransform = GameManager.Instance.getPlayer().transform;

        //Get Floor
        FloorPositions = new HashSet<Vector2Int>(DungeonCreationV2.Instance.GetTilePlaces);
        
        //Create Texture
        int size = viewRadius * 2 + 1;
        Texture2D textureMap = new Texture2D(size, size);
        textureMap.filterMode = FilterMode.Point;
        textureMap.wrapMode = TextureWrapMode.Clamp;

        //Assign UI
        miniMapImage.texture = textureMap;
        mapTexture = textureMap;

    }
    private void GenerateMiniMap()
    {
        //Calc Player Position in the Grid
        Vector2Int playerPos = new Vector2Int(Mathf.RoundToInt(playerTransform.position.x), Mathf.RoundToInt(playerTransform.position.y));

        for(int x = -viewRadius; x <= viewRadius; x++)
        {
            for (int y = -viewRadius; y <= viewRadius; y++)
            {
                Vector2Int CurrentPosition = playerPos + new Vector2Int(x, y);

                bool isFloor = FloorPositions.Contains(CurrentPosition);

                Color col = Color.black;
                if (CurrentPosition == playerPos) { col = Color.red; }
                else if (isFloor) { col = Color.blue; }

                int textureX = x + viewRadius;
                int textureY = y + viewRadius;

                mapTexture.SetPixel(textureX, textureY, col);
            }
        }
        mapTexture.Apply();
    }

    private void Update()
    {
        if(MapUpdateTimer <= 0)
        {
            MapUpdateTimer = MapUpdateInterval;
            GenerateMiniMap();
        }
        else
        {
            MapUpdateTimer -= GameTimeManager.GameDeltaTime;
        }
    }

}
