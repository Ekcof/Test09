using System;

[Serializable]
public class LoadOut
{
    public string Id;
    public ItemData[] ItemDatas;
    public string HelmetId;
    public string UniformId;
}

[Serializable]
public class ItemData
{
    public string Id;
    public int Amount;
}
