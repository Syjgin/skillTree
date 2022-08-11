using Logic;
using UnityEngine;

namespace Data
{
    public class EventHandler : MonoBehaviour
    {
        public delegate void LoadingDataFinished(IInitialDataProvider dataProvider);
        public LoadingDataFinished LoadingDataFinishedEvent;

        public delegate void SaveRequest(SkillTreeState skillTreeState);
        public SaveRequest SaveRequestEvent;
    }
}