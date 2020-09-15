using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{
    //bundle打包格式
    static BuildAssetBundleOptions bbo = BuildAssetBundleOptions.None;
    //bundle打包目标平台
    static BuildTarget bt = BuildTarget.StandaloneWindows;

    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        CreateAssetBundles.bbo,
                                        CreateAssetBundles.bt);
    }
}
