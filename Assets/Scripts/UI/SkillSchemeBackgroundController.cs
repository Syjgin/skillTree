using System;
using System.Collections.Generic;
using Data;
using Logic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class SkillSchemeBackgroundController : MonoBehaviour
    {
        [SerializeField] private float _elementSize;
        [SerializeField] private float _paddingBetweenCircles;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _background;
        [SerializeField] private GameObject _skillButtonPrefab;
        [SerializeField] private LinesRenderer _linesRenderer;
        [SerializeField] private GameObject _baseButtonPrefab;
        [Inject] private EventBus _eventBus;
        private readonly Dictionary<int, List<Skill>> _skillsByCircles = new();
        private readonly Dictionary<int, int> _maxSkillsByCircles = new();
        private readonly Dictionary<int, float> _angleStepsByCircles = new();
        private readonly Dictionary<string, Vector2> _skillPositions = new();

        private void OnEnable()
        {
            _eventBus.SubscribeInitialDataProvider(OnInitialDataLoaded);
        }

        private void OnInitialDataLoaded(IInitialDataProvider dataProvider)
        {
            CalculateRounds(dataProvider.GetAllSkills());
            var radius = GetCircleRadius(_maxSkillsByCircles.Count);
            _background.sizeDelta = new Vector2(radius * 2, radius * 2);
            _scrollRect.horizontalNormalizedPosition = .5f;
            _scrollRect.verticalNormalizedPosition = .5f;
        }

        private float GetCircleRadius(int number)
        {
            return 0.5f * _elementSize + _paddingBetweenCircles +
                   (number + 1) * (_elementSize + _paddingBetweenCircles);
        }

        private void CalculateRounds(IEnumerable<Skill> skills)
        {
            foreach (var skill in skills)
            {
                var currentCircleIndex = skill.Parents.Count;
                if (!_maxSkillsByCircles.ContainsKey(currentCircleIndex))
                {
                    var radius = GetCircleRadius(currentCircleIndex);
                    var circleLength = 2 * Math.PI * radius;
                    var maxElements = Mathf.FloorToInt((float)(circleLength / _elementSize));
                    _maxSkillsByCircles.Add(currentCircleIndex, maxElements);
                    var angleStep = 360f / maxElements;
                    _angleStepsByCircles.Add(currentCircleIndex, angleStep);
                }

                if (!_skillsByCircles.ContainsKey(currentCircleIndex))
                {
                    var list = new List<Skill>
                    {
                        skill
                    };
                    _skillsByCircles.Add(currentCircleIndex, list);
                }
                else
                {
                    var existing = _skillsByCircles[currentCircleIndex];
                    if (existing.Count + 1 == _maxSkillsByCircles[currentCircleIndex])
                    {
                        Debug.LogError("too many elements. Please increase the padding between circles");
                        return;
                    }

                    existing.Add(skill);
                }
            }

            var possiblePositionsByCircles = new Dictionary<int, List<Vector2>>();
            foreach (var angleStepsByCircle in _angleStepsByCircles)
            {
                var radius = GetCircleRadius(angleStepsByCircle.Key);
                for (float angle = 0; angle < 360; angle += angleStepsByCircle.Value)
                {
                    var coords = new Vector2(radius * Mathf.Cos(angle * Mathf.Deg2Rad),
                        radius * Mathf.Sin(angle * Mathf.Deg2Rad));
                    if (possiblePositionsByCircles.ContainsKey(angleStepsByCircle.Key))
                    {
                        var existingList = possiblePositionsByCircles[angleStepsByCircle.Key];
                        existingList.Add(coords);
                    }
                    else
                    {
                        var list = new List<Vector2>
                        {
                            coords
                        };
                        possiblePositionsByCircles.Add(angleStepsByCircle.Key, list);
                    }
                }
            }

            foreach (var skillsByCircle in _skillsByCircles)
            {
                foreach (var skill in skillsByCircle.Value)
                {
                    if (skill.Parents.Count == 0)
                    {
                        var possiblePositions = possiblePositionsByCircles[skillsByCircle.Key];
                        _skillPositions.Add(skill.SkillName, possiblePositions[^1]);
                        possiblePositions.RemoveAt(possiblePositions.Count-1);
                    }
                    else
                    {
                        var firstParent = skill.Parents[0]; //place near the multiple parents may be impossible
                        var parentPosition = _skillPositions[firstParent.SkillName];
                        var distance = float.MaxValue;
                        var preferredPosition = new Vector2();
                        foreach (var possiblePosition in possiblePositionsByCircles[skillsByCircle.Key])
                        {
                            var currentDistance = Vector2.Distance(possiblePosition, parentPosition);
                            if (currentDistance < distance)
                            {
                                distance = currentDistance;
                                preferredPosition = possiblePosition;
                            }
                        }
                        _skillPositions.Add(skill.SkillName, preferredPosition);
                        possiblePositionsByCircles[skillsByCircle.Key].Remove(preferredPosition);
                    }
                }
            }
            
            foreach (var skillsByCircle in _skillsByCircles)
            {
                foreach (var skill in skillsByCircle.Value)
                {
                    if (skillsByCircle.Key == 0)
                    {
                        _linesRenderer.DrawLine(Vector2.zero, _skillPositions[skill.SkillName]);
                    }
                    else
                    {
                        foreach (var skillParent in skill.Parents)
                        {
                            _linesRenderer.DrawLine(_skillPositions[skillParent.SkillName], _skillPositions[skill.SkillName]);
                        }
                    }
                }
            }
            
            foreach (var skillPosition in _skillPositions)
            {
                var instantiated = Instantiate(_skillButtonPrefab, _background, false);
                var rectTransform = instantiated.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = skillPosition.Value;
            }

            var baseButton = Instantiate(_baseButtonPrefab, _background, false);
            var baseRectTransform = baseButton.GetComponent<RectTransform>();
            baseRectTransform.anchoredPosition = Vector2.zero;
        }
    }
}