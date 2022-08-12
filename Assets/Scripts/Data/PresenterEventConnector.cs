using System;
using Logic;
using UnityEngine;
using Zenject;

namespace Data
{
    public class PresenterEventConnector : MonoBehaviour
    {
        [Inject] private EventBus _eventBus;
        private SkillTreePresenter _presenter;
        private void OnEnable()
        {
            _eventBus.SubscribeInitialDataProvider(HandleInitialData);
        }

        private void HandleInitialData(IInitialDataProvider provider)
        {
            _presenter = new SkillTreePresenter(provider);
            _eventBus.SubscribeSkillSelected(OnSkillSelected);
            _eventBus.SubscribeSkillTreeCommand(OnSkillTreeCommand);
        }

        private void OnSkillSelected(Skill skill)
        {
            _presenter.TrySelectSkill(skill.SkillName);
            NotifyStateChanged();
        }

        private void OnSkillTreeCommand(SkillTreeCommand command)
        {
            switch (command.CommandType)
            {
                case SkillTreeCommand.SkillTreeCommandType.Learn:
                    if (_presenter.TryLearn())
                    {
                        NotifyStateChanged();    
                    }
                    break;
                case SkillTreeCommand.SkillTreeCommandType.Forget:
                    if (_presenter.TryForget())
                    {
                        NotifyStateChanged();    
                    }
                    break;
                case SkillTreeCommand.SkillTreeCommandType.ForgetAll:
                    _presenter.ForgetAll();
                    NotifyStateChanged();
                    break;
                case SkillTreeCommand.SkillTreeCommandType.AddSkillPoint:
                    _presenter.AddSkillPoint();
                    NotifyStateChanged();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(command), command, null);
            }
        }

        private void NotifyStateChanged()
        {
            _eventBus.SendStateChanged(new SkillTreeRuntimeState
            {
                CanForget = _presenter.CanForget,
                CanLearn = _presenter.CanLearn,
                State = _presenter.GetState()
            });
        }
    }
}