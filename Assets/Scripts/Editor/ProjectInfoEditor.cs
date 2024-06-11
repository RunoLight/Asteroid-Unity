using UnityEditor;
using UnityEngine;

namespace Asteroid.Editor
{
    [CustomEditor(typeof(ProjectInfo))]
    public class ProjectInfoEditor : UnityEditor.Editor
    {
        private const string AssembliesWarningMessage =
            "Project uses <b> Assembly Definitions </b>, " +
            "in order to watch their description and dependencies - see projects ReadMe";

        private const string ProjectInfoMessage = "Welcome to {0}!\nAuthor: {1}";

        public override void OnInspectorGUI()
        {
            EditorStyles.helpBox.fontSize = 14;
            EditorStyles.helpBox.richText = true;

            GUIStyle buttonStyle = new(GUI.skin.button)
            {
                fixedHeight = 25,
                fontSize = 14
            };

            var projectInfo = (ProjectInfo)target;

            DrawDefaultInspector();

            EditorGUILayout.Separator();

            EditorGUILayout.HelpBox(
                string.Format(ProjectInfoMessage, projectInfo.projectName, projectInfo.author), MessageType.Info
            );

            EditorGUILayout.Separator();

            EditorGUILayout.HelpBox(AssembliesWarningMessage, MessageType.Warning);
            if (GUILayout.Button("Open Project ReadMe", buttonStyle))
            {
                OpenReadme();
            }

            if (GUILayout.Button("Open Project GitHub", buttonStyle))
            {
                OpenProjectGithub();
            }
        }

        private static void OpenReadme()
        {
            Application.OpenURL("https://github.com/RunoLight/Asteroid-Unity");
        }

        private static void OpenProjectGithub()
        {
            Application.OpenURL("https://github.com/RunoLight/Asteroid-Unity/blob/main/README.md");
        }
    }
}