using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

public class Builder
{
    [MenuItem("Great Escape/Build Debug")]
    public static void BuildDebug()
    {
        BuildInternal(false);
    }

    [MenuItem("Great Escape/Build Release")]
    public static void BuildRelease()
    {
        BuildInternal(true);
    }

    static void BuildInternal(bool isRelease)
    {
        var scenePaths = UnityEditor.EditorBuildSettings.scenes
            .Select(x => x.path).ToList();

        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.StandaloneWindows:
            {
                BuildGeneric(
                    "Windows/TheGreatEggscape.exe", scenePaths, isRelease);
                break;
            }
            case BuildTarget.WebGL:
            {
                BuildGeneric("WebGl", scenePaths, isRelease);
                break;
            }
            default:
            {
                throw new Exception(
                    "Cannot build on platform '{0}'".Fmt(EditorUserBuildSettings.activeBuildTarget));
            }
        }
    }

    static bool BuildGeneric(
        string relativePath, List<string> scenePaths, bool isRelease)
    {
        var options = BuildOptions.None;

        var path = Path.Combine(Path.Combine(Application.dataPath, "../../../Builds"), relativePath);

        // Create the directory if it doesn't exist
        // Otherwise the build fails
        Directory.CreateDirectory(Path.GetDirectoryName(path));

        if (!isRelease)
        {
            options |= BuildOptions.Development;
        }

        var buildResult = BuildPipeline.BuildPlayer(scenePaths.ToArray(), path, EditorUserBuildSettings.activeBuildTarget, options);

#if UNITY_2018_1_OR_NEWER
        bool succeeded = (buildResult.summary.result == BuildResult.Succeeded);
#else
        bool succeeded = (buildResult.Length == 0);
#endif

        if (succeeded)
        {
            Log.Info("Build completed successfully");
        }
        else
        {
            Log.Error("Error occurred while building");
        }

        if (UnityEditorInternal.InternalEditorUtility.inBatchMode)
        {
            EditorApplication.Exit(succeeded ? 0 : 1);
        }

        return succeeded;
    }
}

