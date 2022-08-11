using Data;
using Zenject;

namespace Installers
{
    public class EventBusInstaller : MonoInstaller<EventBusInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInstance(new EventBus()).AsSingle();
        }
    }
}