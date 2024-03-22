namespace HRMS.Helpers
{
    using System;

    public class WorkloadMetrics
    {
        public int TasksCompleted { get; set; }
        public int TasksInProgress { get; set; }
        public int PendingTasks { get; set; }
        public double WorkloadIndex { get; private set; }

        public WorkloadMetrics(int completed, int inProgress, int pending)
        {
            TasksCompleted = completed;
            TasksInProgress = inProgress;
            PendingTasks = pending;
            CalculateWorkloadIndex();
        }

        // Method to calculate workload index
        private void CalculateWorkloadIndex()
        {
            //if ((TasksCompleted + TasksInProgress + PendingTasks) == 0)
            //{
            //    WorkloadIndex = 0;
            //}
            //else
            //{
            //    WorkloadIndex = (TasksCompleted + (TasksInProgress * 0.5)) / (TasksCompleted + TasksInProgress + PendingTasks + 1);
            //}
            double totalTasks = TasksCompleted + TasksInProgress + PendingTasks;
            if (totalTasks > 0)
            {
                double weightedCompleted = TasksCompleted * 1.0; // Weight for completed tasks
                double weightedInProgress = TasksInProgress * 0.8; // Weight for tasks in progress
                double weightedPending = PendingTasks * 0.5; // Weight for pending tasks

                // Considering priority of tasks
                weightedCompleted *= CalculatePriorityFactor(TaskPriority.High);
                weightedInProgress *= CalculatePriorityFactor(TaskPriority.Medium);
                weightedPending *= CalculatePriorityFactor(TaskPriority.Low);

                WorkloadIndex = (weightedCompleted + weightedInProgress + weightedPending) / totalTasks;
            }
            else
            {
                WorkloadIndex = 0;
            }
        }
        private double CalculatePriorityFactor(TaskPriority priority)
        {
            // Assigning weight based on priority
            switch (priority)
            {
                case TaskPriority.High:
                    return 1.2; // High priority tasks have a higher weight
                case TaskPriority.Medium:
                    return 1.0; // Medium priority tasks have a standard weight
                case TaskPriority.Low:
                    return 0.8; // Low priority tasks have a lower weight
                default:
                    return 1.0; // Default weight
            }
        }

        // Enum to represent task priority
        public enum TaskPriority
        {
            Low,
            Medium,
            High
        }

    }


}
