using CalculatorIP.Model.Task.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorIP
{
    public class Session
    {
        private Dictionary<SelectedTask, TaskBase> _tasks = new Dictionary<SelectedTask, TaskBase>(); 
        private Dictionary<SelectedTask, GeneratedTask> _lastGeneratedTasks = new Dictionary<SelectedTask, GeneratedTask>();

        public Session()
        {
            InitializeTasks();
            InitializeLastGeneratedTasks();
        }

        private void InitializeLastGeneratedTasks()
        {
            _lastGeneratedTasks.Add(SelectedTask.First, new GeneratedTask(null, null, null));
            _lastGeneratedTasks.Add(SelectedTask.Second, new GeneratedTask(null, null, null));
            _lastGeneratedTasks.Add(SelectedTask.Third, new GeneratedTask(null, null, null));
        }

        private void InitializeTasks()
        {
            _tasks.Add(SelectedTask.First, new HostsCount());
            _tasks.Add(SelectedTask.Second, new SubnetsCount());
            _tasks.Add(SelectedTask.Third, new SubnetOctetCalculator());
        }

        public GeneratedTask GetNewTask(SelectedTask selectedTask)
        {
            var task = _tasks[selectedTask].GetTask();
            _lastGeneratedTasks[selectedTask] = task;
            return task;
        }

        public GeneratedTask GetGeneratedTask(SelectedTask taskNumber)
        {
            return _lastGeneratedTasks[taskNumber];
        }

        public class GeneratedTask
        {
            public string Task { get; private set; }

            public string Description { get; private set; }

            public string Result { get; private set; }

            public GeneratedTask(string task, string result, string description)
            {
                Task = task;
                Description = description;
                Result = result;
            }
        }

        public enum SelectedTask
        {
            None, 
            First,
            Second,
            Third
        }
    }
}
