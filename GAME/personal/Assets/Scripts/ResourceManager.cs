using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public static Object LoadFromFile()
    {
        return AssetBundleManager.LoadFromFile();
    }
}
