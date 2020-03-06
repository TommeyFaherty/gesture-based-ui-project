using System.IO;
using UnityEngine;

// very losely based on this: https://youtu.be/B_Xp9pt8nRY
public class LevelBuilder : MonoBehaviour
{
    public string levelBlocksPath;
    public string levelRotationsPath;
    public BlockCharacter[] blockMappings;

    void Start()
    {
        BuildLevel();
    }

    void BuildLevel()
    {
        StreamReader blocksReader = new StreamReader("Assets/Levels/" + levelBlocksPath);
        StreamReader rotationsReader = new StreamReader("Assets/Levels/" + levelRotationsPath);
        // read width/length/height
        string[] dims = blocksReader.ReadLine().Split();
        rotationsReader.ReadLine();
        int width = int.Parse(dims[0]);
        int length = int.Parse(dims[1]);
        int height = int.Parse(dims[2]);
        char[] blocksLine, rotationsLine;

        for (int h = 0; h < height; h++)
        {
            for (int l = 0; l < length; l++)
            {
                blocksLine = blocksReader.ReadLine().ToCharArray();
                rotationsLine = rotationsReader.ReadLine().ToCharArray();
                for (int w = 0; w < blocksLine.Length; w++)
                {
                    char b = blocksLine[w];
                    int r = 0;
                    if (w < rotationsLine.Length)
                    {
                        r = (int) char.GetNumericValue(rotationsLine[w]);
                    }
                    GenerateBlock(b, w * 2, h * 2, -l * 2, r);
                }
            }

            // skip blank line after each layer
            blocksReader.ReadLine();
            rotationsReader.ReadLine();
        }
       
        blocksReader.Close();
        rotationsReader.Close();
    }

    void GenerateBlock(char b, int x, int y, int z, int r)
    {
        if (b == ' ')
        {
            // ignore empty space block
            return;
        }

        Debug.Log("z: " + z);

        foreach (BlockCharacter blockMapping in blockMappings)
        {
            if (blockMapping.character == b)
            {
                Vector3 position = new Vector3(x, y, z);
                Quaternion rotation = blockMapping.block.transform.rotation * Quaternion.Euler(r * 90, 0, 0);
                GameObject block = Instantiate(blockMapping.block, position, rotation, transform);
                block.transform.parent = gameObject.transform;
            }
        }
    }
}
