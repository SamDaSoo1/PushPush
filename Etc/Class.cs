using System.Collections.Generic;

[System.Serializable]
public class StageData
{
    public List<int> idx;
    public List<BlockType> blockType;
}

public class SavePoint
{
    public int lastClear;
}