using System.IO;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public string levelBlocksPath;
    public BlockCharacter[] blockMappings;

    void Start()
    {
        BuildLevel();
    }

    void BuildLevel()
    {
        StreamReader reader = new StreamReader("Assets/Levels/" + levelBlocksPath);
        // read width/length/height
        string[] dims = reader.ReadLine().Split();
        int width = int.Parse(dims[0]);
        int length = int.Parse(dims[1]);
        int height = int.Parse(dims[2]);
        char[] line;

        for (int h = 0; h < width; h++)
        {
            for (int l = 0; l < length; l++)
            {
                line = reader.ReadLine().ToCharArray();
                for (int w = 0; w < line.Length; w++)
                {
                    char c = line[w];
                    GenerateBlock(c, w, l, h);
                }
            }

            // skip blank line after each layer
            reader.ReadLine();
        }

        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }

    void GenerateBlock(char c, int x, int y, int z)
    {
        if (c == ' ')
        {
            // ignore empty space block
            return;
        }

        foreach (BlockCharacter blockMapping in blockMappings)
        {
            if (blockMapping.character == c)
            {
                Vector2 position = new Vector2(x, y);
                Instantiate(blockMapping.block, position, Quaternion.identity, transform);
            }
        }
    }
}
