using UnityEngine;
using YG;

namespace YG
{
    public partial class SavesYG
    {
        public int LevelCounter;
    }
}

namespace Puzzle3D
{
    public class MetricaManager : MonoBehaviour
    {
        public static MetricaManager I { get; private set; }

        [SerializeField] private string startGoalName = "start";
        [SerializeField] private string levelCompleteGoalName = "levelComplete";

        private void Awake()
        {
            if (I != null)
            {
                Destroy(gameObject);

                return;
            }

            I = this;

            DontDestroyOnLoad(gameObject);

            YG2.MetricaSend(startGoalName);
        }

        public void SendLevelCompleteMessage()
        {
            YG2.saves.LevelCounter++;

            YG2.MetricaSend(levelCompleteGoalName, "level", YG2.saves.LevelCounter.ToString());
        }
    }
}