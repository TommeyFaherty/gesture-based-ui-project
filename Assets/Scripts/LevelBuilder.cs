using UnityEngine;

// based on this tutorial https://youtu.be/B_Xp9pt8nRY
public class LevelBuilder : MonoBehaviour
{
    public Texture2D map;
    public BlockColour[] blockMappings;

    void Start()
    {
        BuildLevel();
    }

    void BuildLevel()
    {
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                GenerateBlock(x, y);
            }
        }
    }

    void GenerateBlock(int x, int y)
    {
        Color pixelColor = map.GetPixel(x, y);

        if (pixelColor.a == 0)
        {
            // ignore transparent pixels
            return;
        }

        foreach (BlockColour blockMapping in blockMappings)
        {
            if (blockMapping.color.Equals(pixelColor))
            {
                Vector2 position = new Vector2(x, y);
                Instantiate(blockMapping.prefab, position, Quaternion.identity, transform);
            }
        }
    }
}
