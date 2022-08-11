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
            presenter.SelectSkill("Two");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.Learn();
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
            presenter.SelectSkill("Four");
            Assert.AreEqual(false, presenter.CanLearn);
            presenter.SelectSkill("Two");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.Learn();
            Assert.AreEqual(0, presenter.SkillPoints);
            Assert.AreEqual(true, presenter.IsSkillKnown("Two"));
            presenter.SelectSkill("Four");
            presenter.AddSkillPoint();
            presenter.AddSkillPoint();
            presenter.AddSkillPoint();
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.Learn();
            Assert.AreEqual(true, presenter.IsSkillKnown("Four"));
            var state = presenter.GetState();
            Assert.AreEqual(3, state.KnownSkills.Count);
            Assert.AreEqual(0, state.FreeSkillPoints);
        }

        [Test]
        public void SimpleForgetTest()
        {
            var presenter = new SkillTreePresenter(new TestInitialDataProvider());
            presenter.SelectSkill("One");
            Assert.AreEqual(true, presenter.CanForget);
            presenter.SelectSkill("Two");
            Assert.AreEqual(false, presenter.CanForget);
            presenter.SelectSkill("One");
            presenter.Forget();
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
            presenter.SelectSkill("Two");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.Learn();
            presenter.SelectSkill("Three");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.Learn();
            for (int i = 0; i < 3; i++)
            {
                presenter.AddSkillPoint();
            }
            presenter.SelectSkill("Four");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.Learn();
            Assert.AreEqual(true, presenter.CanForget);
            presenter.SelectSkill("Two");
            Assert.AreEqual(false, presenter.CanForget);
            presenter.Learn();
            presenter.SelectSkill("Three");
            Assert.AreEqual(false, presenter.CanForget);
            presenter.SelectSkill("Four");
            presenter.Forget();
            presenter.SelectSkill("Two");
            Assert.AreEqual(true, presenter.CanForget);
            presenter.SelectSkill("Three");
            Assert.AreEqual(true, presenter.CanForget);
        }

        [Test]
        public void ForgetAllTest()
        {
            var initialDataProvider = new TestInitialDataProvider();
            var presenter = new SkillTreePresenter(initialDataProvider);
            presenter.SelectSkill("One");
            Assert.AreEqual(true, presenter.CanForget);
            presenter.Forget();
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
            
            presenter.SelectSkill("One");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.Learn();
            Assert.AreEqual(1, presenter.GetState().KnownSkills.Count);
            
            presenter.SelectSkill("Two");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.Learn();
            Assert.AreEqual(2, presenter.GetState().KnownSkills.Count);
            
            presenter.SelectSkill("Three");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.Learn();
            Assert.AreEqual(3, presenter.GetState().KnownSkills.Count);
            
            presenter.SelectSkill("Four");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.Learn();
            Assert.AreEqual(4, presenter.GetState().KnownSkills.Count);
            
            presenter.SelectSkill("Five");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.Learn();
            Assert.AreEqual(5, presenter.GetState().KnownSkills.Count);
            
            presenter.SelectSkill("Six");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.Learn();
            Assert.AreEqual(6, presenter.GetState().KnownSkills.Count);
            
            presenter.SelectSkill("Seven");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.Learn();
            Assert.AreEqual(7, presenter.GetState().KnownSkills.Count);
            
            presenter.SelectSkill("Eight");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.Learn();
            Assert.AreEqual(8, presenter.GetState().KnownSkills.Count);
            
            presenter.SelectSkill("Nine");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.Learn();
            Assert.AreEqual(9, presenter.GetState().KnownSkills.Count);
            
            presenter.SelectSkill("Ten");
            Assert.AreEqual(true, presenter.CanLearn);
            presenter.Learn();
            Assert.AreEqual(10, presenter.GetState().KnownSkills.Count);
            
            presenter.ForgetAll();
            Assert.AreEqual(0, presenter.GetState().KnownSkills.Count);
            Assert.AreEqual(neededSkillPoints + 1, presenter.SkillPoints);
            
        }
    }
}
