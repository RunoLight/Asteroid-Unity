using UnityEngine;

namespace Asteroid.Editor
{
    [CreateAssetMenu(fileName = "ProjectInfo", menuName = "Project Info", order = 1)]
    public class ProjectInfo : ScriptableObject
    {
        public string projectName;
        public string author;
    }
}