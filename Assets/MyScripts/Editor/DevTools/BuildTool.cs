/// reference : https://qiita.com/sango/items/474efb4c016a136c84ce
/// official Document:


using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.IO;

public static class BuildTool
{
    private static string projectName = Application.productName;

    [MenuItem("BuildTool/Windows(Develop)")]
    public static void Build_Win64_Develop()
        => BuildWindows(true);

    [MenuItem("BuildTool/Windows(Release)")]
    public static void Build_Win64_Release()
        => BuildWindows(false);

    [MenuItem("BuildTool/Android(Develop)")]
    public static void Build_Android_Develop()
        => BuildAndroid(true);

    [MenuItem("BuildTool/Android(Release)")]
    public static void Build_Android_Release()
        => BuildAndroid(false);

    private static void BuildWindows(bool isDevelop)
    {
        var buildOptions = new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes.Select(s => s.path).ToArray(),
            locationPathName = $"Builds\\Windows\\{Application.productName}.exe", 
            target = BuildTarget.StandaloneWindows64,
            options = isDevelop ? BuildOptions.Development : BuildOptions.AllowDebugging
        };
            
        Build(buildOptions);
     
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);

        Debug.Log($@"
===Player Settings===
id = {PlayerSettings.applicationIdentifier}
version = {PlayerSettings.bundleVersion}
=====================
");

        // バンドルアセットをコピー
        CopyDirectory("BundleAssets", "Builds/Windows", true);
        CopyDirectory("UserData", "Builds/Windows/UserData", true);

        // IL2CPPの場合シンボルファイルを外に移動
        if (PlayerSettings.GetScriptingBackend(BuildTargetGroup.Standalone) != ScriptingImplementation.IL2CPP) 
            return;
        
        var dest = "Builds/Windows_IL2CPPBackup";
        if (Directory.Exists(dest))
        {
            Directory.Delete(dest, true);
        }

        Directory.Move($"Builds/Windows/{projectName}_BackUpThisFolder_ButDontShipItWithYourGame", dest);
    }

    private static void BuildAndroid(bool isDevelop)
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            throw new Exception("Change Build Target And Retry");
        }
        
        EditorUserBuildSettings.androidBuildSubtarget
            = MobileTextureSubtarget.ASTC;
        EditorUserBuildSettings.androidBuildSystem
            = AndroidBuildSystem.Gradle;

        if (PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android) == ScriptingImplementation.IL2CPP)
        {
            PlayerSettings.bundleVersion = PlayerSettings.bundleVersion.Replace('/', '_');
        }

        Debug.Log($@"
===Player Settings===
id = {PlayerSettings.applicationIdentifier}
version = {PlayerSettings.bundleVersion}
bundle version code = {PlayerSettings.Android.bundleVersionCode}
=====================
");
        
        SignApk();

        var buildOptions = new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes.Select(s => s.path).ToArray(),
            locationPathName = $"Builds\\Windows\\{Application.productName}.exe", 
            target = BuildTarget.StandaloneWindows64,
            options = isDevelop ? BuildOptions.Development : BuildOptions.AllowDebugging
        };
            
        Build(buildOptions);
        
        
        Debug.Log("Output files");
        foreach (string file in Directory.GetFiles("Builds/Android"))
        {
            Debug.Log(file);
        }

        UnsignApk();
    }

    private static void Build(BuildPlayerOptions option)
    {
        if (!PlayerSettings.MTRendering)
        {
            Debug.Log("Multithread renderingが無効になっています。再度有効にします。");
            PlayerSettings.MTRendering = true;
        }
        
        var report = BuildPipeline.BuildPlayer(option);
        if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            throw new Exception("Build Not Succeeded");
    }

    private static void SignApk()
    {
        var storeName = Environment.GetEnvironmentVariable("APK_KEYSTORE_NAME");
        var storePass = Environment.GetEnvironmentVariable("APK_KEYSTORE_PASS");

        var aliasName = Environment.GetEnvironmentVariable("APK_ALIAS_NAME");
        var aliasPass = Environment.GetEnvironmentVariable("APK_ALIAS_PASS");

        PlayerSettings.Android.keystoreName = storeName;
        PlayerSettings.Android.keystorePass = storePass;
        PlayerSettings.Android.keyaliasName = aliasName;
        PlayerSettings.Android.keyaliasPass = aliasPass;
    }

    private static void UnsignApk()
    {
        PlayerSettings.Android.keystoreName = default;
        PlayerSettings.Android.keystorePass = default;
        PlayerSettings.Android.keyaliasName = default;
        PlayerSettings.Android.keyaliasPass = default;
    }

    private static void CopyDirectory(string source, string dest, bool overwrite)
    {
        if (!Directory.Exists(dest))
        {
            Directory.CreateDirectory(dest);
            File.SetAttributes(dest, File.GetAttributes(source));
            overwrite = true;
        }

        foreach (string srcFile in Directory.GetFiles(source))
        {
            string destFile = Path.Combine(dest, Path.GetFileName(srcFile));
            if (!overwrite && File.Exists(destFile))
                continue;
            File.Copy(srcFile, destFile, overwrite);
        }

        foreach (string sourceSubDirectory in Directory.GetDirectories(source))
        {
            string destSubDirectory = Path.Combine(dest, Path.GetFileName(sourceSubDirectory) ?? throw new NullReferenceException($"sourceSubDirectory is not set"));
            CopyDirectory(sourceSubDirectory, destSubDirectory, overwrite);
        }
    }
}