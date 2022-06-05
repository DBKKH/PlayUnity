using UnityEngine;
using System;
using TMPro;
using UniRx;
using UnityEngine.UI;

namespace UniRxSample
{
    public sealed class SampleReactiveCollection : MonoBehaviour
    {
        [SerializeField] Button _addButton, _moveButton, _replacedButton, _removeButton;
        [SerializeField] TestTask[] _addedTasks;
        [SerializeField] TestTask _replacedTasks;
        [SerializeField] int _targetIndex = 0, _oldIndex = 0, _newIndex = 2;

        ReactiveCollection<TestTask> _taskCollection = new ReactiveCollection<TestTask>();

        void Start()
        {
            InitButtons();
            SetObservers();
        }

        void InitButtons()
        {
            _addButton.onClick.AddListener(() =>
            {
                for (var i = 0; i < _addedTasks.Length; i++)
                {
                    _taskCollection.Add(_addedTasks[i]);
                }
            });

            _moveButton.onClick.AddListener(() => { _taskCollection.Move(_oldIndex, _newIndex); });

            _replacedButton.onClick.AddListener(() => { _taskCollection[_targetIndex] = _replacedTasks; });

            _removeButton.onClick.AddListener(() => { _taskCollection.RemoveAt(_targetIndex); });
        }

        void SetObservers()
        {
            _taskCollection
                .ObserveAdd()
                .Subscribe(value => { Debug.Log($"[Add]Index={value.Index},Value={value.Value}"); });

            _taskCollection
                .ObserveMove()
                .Subscribe(value =>
                {
                    Debug.Log($"[Move]Value={value.Value},NewIndex={value.NewIndex},OldIndex={value.OldIndex}");
                });

            _taskCollection
                .ObserveRemove()
                .Subscribe(value => { Debug.Log($"[Remove]Index={value.Index},Value={value.Value}"); });

            _taskCollection
                .ObserveReplace()
                .Subscribe(value =>
                {
                    Debug.Log($"[Replace]Index={value.Index},NewValue={value.NewValue},OldValue={value.OldValue}");
                });

            _taskCollection
                .ObserveReset()
                .Subscribe(value => { Debug.Log($"[Reset]"); });
        }
    }

    [Serializable]
    public sealed class TestTask
    {
        public int Id = 0;
        public string Title = "title";
        public TaskStatus Status = TaskStatus.NotStarted;
        public int TimeStamp { get; private set; }

        public TestTask(int id, string title, TaskStatus status)
        {
            Id = id;
            Title = title;
            Status = status;
            TimeStamp = DateTime.Now.Second;
        }
    }

    public enum TaskStatus
    {
        NotStarted,
        Pending,
        InProcess,
        Completed,
        Impossible,
        CallingSupport,
        Cancelled,
        Failed,
    }
}