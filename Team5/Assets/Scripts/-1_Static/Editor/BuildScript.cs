using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

public class BuildScript : MonoBehaviour
{
    [MenuItem("Build/Build Windows")]
    public static void MyBuild_Windows()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] {
            "Assets/Scenes/1_Lobby.unity",
            "Assets/Scenes/2_CutScene.unity",
            "Assets/Scenes/3_Main.unity",
            "Assets/Scenes/4_UnderWorld.unity",
        };
        buildPlayerOptions.locationPathName = "Build/Game.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
}