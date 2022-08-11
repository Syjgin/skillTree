using Data;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class EventBusInstaller : MonoInstaller<EventBusInstaller>
    {
        [SerializeField] private EventBus _eventBus;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_eventBus);
        }
    }
}