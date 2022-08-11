using System;
using Logic;
using UniRx;

namespace Data
{
    public class EventBus
    {
        private readonly BehaviorSubject<IInitialDataProvider> _dataProviderSubject = new(null);

        public void SubscribeInitialDataProvider(Action<IInitialDataProvider> callback)
        {
            _dataProviderSubject.SkipWhile(it => it == null)
                .Subscribe(callback);
            if(_dataProviderSubject.Value != null)
                _dataProviderSubject.PublishLast();
        }

        public void SendInitialDataProvider(IInitialDataProvider initialDataProvider)
        {
            _dataProviderSubject.OnNext(initialDataProvider);
            _dataProviderSubject.PublishLast();
        }

        public void SubscribeStateChanged(Action<SkillTreeRuntimeState> callback)
        {
            MessageBroker.Default
                .Receive<SkillTreeRuntimeState>()
                .Subscribe(callback);
        }

        public void SendStateChanged(SkillTreeRuntimeState state)
        {
            MessageBroker.Default.Publish(state);
        }

        public void SubscribeSkillSelected(Action<Skill> callback)
        {
            MessageBroker.Default
                .Receive<Skill>()
                .Subscribe(callback);
        }

        public void SelectSkill(Skill skill)
        {
            MessageBroker.Default.Publish(skill);
        }

        public void SubscribeSkillTreeCommand(Action<SkillTreeCommand> callback)
        {
            MessageBroker.Default
                .Receive<SkillTreeCommand>()
                .Subscribe(callback);
        }

        public void SendCommand(SkillTreeCommand command)
        {
            MessageBroker.Default.Publish(command);
        }
    }
}