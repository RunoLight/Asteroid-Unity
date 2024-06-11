using UnityEditor;
using UnityEngine;

namespace Asteroid.Editor
{
    [InitializeOnLoad]
    public class ProjectInfoInitializer
    {
        static ProjectInfoInitializer()
        {
            EditorApplication.delayCall += ShowProjectInfo;
        }

        private static void ShowProjectInfo()
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(ProjectInfo)}");
            if (guids.Length == 0)
            {
                Debug.LogWarning("ProjectInfo asset not found.");
                return;
            }

            var projectInfo = AssetDatabase.LoadAssetAtPath<ProjectInfo>(AssetDatabase.GUIDToAssetPath(guids[0]));

            if (projectInfo == null)
                return;

            Selection.activeObject = projectInfo;
            EditorGUIUtility.PingObject(projectInfo);
        }
    }
}