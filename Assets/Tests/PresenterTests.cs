using System.Collections.Generic;
using Logic;
using NUnit.Framework;

namespace Tests
{
    public class PresenterTests
    {
        [Test]
        public void SkillKnownDetectionTest()
        {
            var presenter = new SkillTreePresenter(new TestInitialDataProvider());
            Assert.AreEqual(true, presenter.IsSkillKnown("One"));
            Assert.AreEqual(false, presenter.IsSkillKnown("dfsfdsdf"));
            Assert.AreEqual(false, presenter.IsSkillKnown("Two"));
            var newState = presenter.GetState();
            Assert.AreEqual(1, newState.KnownSkills.Count);
            Assert.AreEqual("One", newState.KnownSkills[0]);
        }
        
        [Test]
        public void SkillPointAddTest()
        {
            var presenter = new SkillTreePresenter(new TestInitialDataProvider());
            Assert.AreEqual(0, presenter.SkillPoints);
            presenter.AddSkillPoint();
            Assert.AreEqual(1, presenter.SkillPoints);
            var newState = presenter.GetState();
            Assert.AreEqual(1, newState.FreeSkillPoints);
        }

        [Test]
        public void SimpleLearnTest()
        {
            var presenter = new SkillTreePresenter(new TestInitialDataProvider());
            Assert.AreEqual(false, presenter.CanLearn);
            presenter.AddSkillPoint();
            presenter.AddSkillPoint();
            Assert.AreEqual(false, presenter.CanLearn);
            presenter.TrySelectSkill("Two");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.TryLearn();
            Assert.AreEqual(0, presenter.SkillPoints);
            Assert.AreEqual(true, presenter.IsSkillKnown("Two"));
            var state = presenter.GetState();
            Assert.AreEqual(2, state.KnownSkills.Count);
            Assert.AreEqual(0, state.FreeSkillPoints);
        }
        
        [Test]
        public void DependencyLearnTest()
        {
            var presenter = new SkillTreePresenter(new TestInitialDataProvider());
            Assert.AreEqual(false, presenter.CanLearn);
            presenter.AddSkillPoint();
            presenter.AddSkillPoint();
            presenter.TrySelectSkill("Four");
            Assert.AreEqual(false, presenter.CanLearn);
            presenter.TrySelectSkill("Two");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.TryLearn();
            Assert.AreEqual(0, presenter.SkillPoints);
            Assert.AreEqual(true, presenter.IsSkillKnown("Two"));
            presenter.TrySelectSkill("Four");
            presenter.AddSkillPoint();
            presenter.AddSkillPoint();
            presenter.AddSkillPoint();
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.TryLearn();
            Assert.AreEqual(true, presenter.IsSkillKnown("Four"));
            var state = presenter.GetState();
            Assert.AreEqual(3, state.KnownSkills.Count);
            Assert.AreEqual(0, state.FreeSkillPoints);
        }

        [Test]
        public void SimpleForgetTest()
        {
            var presenter = new SkillTreePresenter(new TestInitialDataProvider());
            presenter.TrySelectSkill("One");
            Assert.AreEqual(true, presenter.CanForget);
            presenter.TrySelectSkill("Two");
            Assert.AreEqual(false, presenter.CanForget);
            presenter.TrySelectSkill("One");
            presenter.TryForget();
            Assert.AreEqual(false, presenter.IsSkillKnown("One"));
            var state = presenter.GetState();
            Assert.AreEqual(0, state.KnownSkills.Count);
            Assert.AreEqual(1, state.FreeSkillPoints);
        }

        [Test]
        public void DependencyForgetTest()
        {
            var presenter = new SkillTreePresenter(new TestInitialDataProvider());
            for (int i = 0; i < 5; i++)
            {
                presenter.AddSkillPoint();
            }
            presenter.TrySelectSkill("Two");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.TryLearn();
            presenter.TrySelectSkill("Three");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.TryLearn();
            for (int i = 0; i < 3; i++)
            {
                presenter.AddSkillPoint();
            }
            presenter.TrySelectSkill("Four");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.TryLearn();
            Assert.AreEqual(true, presenter.CanForget);
            presenter.TrySelectSkill("Two");
            Assert.AreEqual(false, presenter.CanForget);
            presenter.TryLearn();
            presenter.TrySelectSkill("Three");
            Assert.AreEqual(false, presenter.CanForget);
            presenter.TrySelectSkill("Four");
            presenter.TryForget();
            presenter.TrySelectSkill("Two");
            Assert.AreEqual(true, presenter.CanForget);
            presenter.TrySelectSkill("Three");
            Assert.AreEqual(true, presenter.CanForget);
        }

        [Test]
        public void ForgetAllTest()
        {
            var initialDataProvider = new TestInitialDataProvider();
            var presenter = new SkillTreePresenter(initialDataProvider);
            presenter.TrySelectSkill("One");
            Assert.AreEqual(true, presenter.CanForget);
            presenter.TryForget();
            Assert.AreEqual(1, presenter.SkillPoints);
            var neededSkillPoints = 0;
            foreach (var skill in initialDataProvider.GetAllSkills())
            {
                neededSkillPoints += skill.SkillPointsToLearn;
            }

            for (int i = 0; i < neededSkillPoints; i++)
            {
                presenter.AddSkillPoint();
            }
            Assert.AreEqual(neededSkillPoints + 1, presenter.SkillPoints);
            
            presenter.TrySelectSkill("One");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.TryLearn();
            Assert.AreEqual(1, presenter.GetState().KnownSkills.Count);
            
            presenter.TrySelectSkill("Two");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.TryLearn();
            Assert.AreEqual(2, presenter.GetState().KnownSkills.Count);
            
            presenter.TrySelectSkill("Three");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.TryLearn();
            Assert.AreEqual(3, presenter.GetState().KnownSkills.Count);
            
            presenter.TrySelectSkill("Four");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.TryLearn();
            Assert.AreEqual(4, presenter.GetState().KnownSkills.Count);
            
            presenter.TrySelectSkill("Five");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.TryLearn();
            Assert.AreEqual(5, presenter.GetState().KnownSkills.Count);
            
            presenter.TrySelectSkill("Six");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.TryLearn();
            Assert.AreEqual(6, presenter.GetState().KnownSkills.Count);
            
            presenter.TrySelectSkill("Seven");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.TryLearn();
            Assert.AreEqual(7, presenter.GetState().KnownSkills.Count);
            
            presenter.TrySelectSkill("Eight");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.TryLearn();
            Assert.AreEqual(8, presenter.GetState().KnownSkills.Count);
            
            presenter.TrySelectSkill("Nine");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.TryLearn();
            Assert.AreEqual(9, presenter.GetState().KnownSkills.Count);
            
            presenter.TrySelectSkill("Ten");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.TryLearn();
            Assert.AreEqual(10, presenter.GetState().KnownSkills.Count);
            
            presenter.ForgetAll();
            Assert.AreEqual(0, presenter.GetState().KnownSkills.Count);
            Assert.AreEqual(neededSkillPoints + 1, presenter.SkillPoints);
            
        }
    }
}
