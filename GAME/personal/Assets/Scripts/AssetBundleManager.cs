using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetBundleManager
{
    //从本地存储加载
    public static Object LoadFromFile()
    {
        var myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/my.other");
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return null;
        }
        var sp = myLoadedAssetBundle.LoadAsset<Sprite>("Assets/64 flat icons/Elements_Air.png");
        return sp;
    }
}
