using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// very losely based on this: https://youtu.be/B_Xp9pt8nRY
public class LevelBuilder : MonoBehaviour
{
    // path of block layout file
    public string levelBlocksPath;
    // path of block rotations file
    public string levelRotationsPath;
    // mapping from character to block (specified in the editor)
    public BlockCharacter[] blockMappings;

    void Start()
    {
        BuildLevel();
    }

    void BuildLevel()
    {
        // open layout files
        string levelDataRoot = Application.dataPath + "/Levels/";
        StreamReader blocksReader = new StreamReader(levelDataRoot + levelBlocksPath);
        StreamReader rotationsReader = new StreamReader(levelDataRoot + levelRotationsPath);
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
                // read line of blocks/rotations
                blocksLine = blocksReader.ReadLine().ToCharArray();
                rotationsLine = rotationsReader.ReadLine().ToCharArray();
                for (int w = 0; w < blocksLine.Length; w++)
                {
                    // current block
                    char b = blocksLine[w];
                    int r = 0;
                    if (w < rotationsLine.Length)
                    {
                        // block rotation
                        r = (int) char.GetNumericValue(rotationsLine[w]);
                    }
                    // instantiate block in the world
                    GenerateBlock(b, w * 2, h, -l * 2, r);
                }
            }

            // skip blank line after each layer
            blocksReader.ReadLine();
            rotationsReader.ReadLine();
        }
        // close files
        blocksReader.Close();
        rotationsReader.Close();

        // move blocks to be centered for this GameObject, based on the midpoint of all child blocks
        CentreBlocksInLevel();
    }

    void GenerateBlock(char b, int x, int y, int z, int r)
    {
        if (b == ' ')
        {
            // ignore empty space block
            return;
        }

        // find the corresponding block for the specified character
        foreach (BlockCharacter blockMapping in blockMappings)
        {
            if (blockMapping.character == b)
            {
                // spawn block
                Vector3 position = new Vector3(x, y, z);
                Quaternion rotation = blockMapping.block.transform.rotation * Quaternion.Euler(r * 90, 0, 0);
                GameObject block = Instantiate(blockMapping.block, position, rotation, transform);
                // set block's parent to the level object
                block.transform.parent = gameObject.transform;
            }
        }
    }

    private void CentreBlocksInLevel()
    {
        Vector3 oldCentre = transform.position;
        Vector3 newCentre = Vector3.zero;
        // add up all child positions
        foreach (Transform child in transform)
        {
            newCentre += child.position;
        }
        // compute the center (mean position)
        newCentre /= transform.childCount;
        // distance to move each child
        Vector3 childOffset = newCentre - oldCentre;

        // move children
        foreach (Transform child in transform)
        {
            child.transform.position -= childOffset;
        }
    }
}
