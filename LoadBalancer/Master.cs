using System.Text;

namespace LoadBalancer;

public class Master
{
    public List<Slave> Slaves { get; private set; }
    private readonly Queue<Work> _taskQueue;

    public Master(int nbrSlaves, int nbrInstances)
    {
        _taskQueue = new Queue<Work>();
        Slaves = new List<Slave>();

        for (var i = 0; i < nbrSlaves; i++)
        {
            var slave = new Slave(this, nbrInstances);
            Slaves.Add(slave);
        }
    }

    public void ReceiveTask(Work work)
    {
        var availableSlave = GetLessBusySlave();

        if (availableSlave != null)
            availableSlave.AssignTask(work);
        else
            _taskQueue.Enqueue(work);

        DisplaySlavesUsage();
    }

    public Slave GetLessBusySlave()
    {
        var lessBusySlave = Slaves.OrderByDescending(s => s.GetAvailableInstanceCount()).First();
        return lessBusySlave;
    }

    public void NotifyTaskCompleted()
    {
        if (_taskQueue.Count > 0)
        {
            var nextTask = _taskQueue.Dequeue();
            ReceiveTask(nextTask);
        }
    }

    public void AddTaskToQueue(Work work)
    {
        _taskQueue.Enqueue(work);
    }

    public double GetQueueSize()
    {
        return _taskQueue.Count;
    }

    public string DisplaySlavesUsage()
    {
        var sb = new StringBuilder();

        sb.AppendLine("\r\n---------------------");

        for (var i = 0; i < Slaves.Count; i++)
            sb.AppendLine($" Slave {i + 1}: \r\n{Slaves[i].ToString()}");

        sb.AppendLine($"Queue: {GetQueueSize()}");

        sb.AppendLine("\r\n---------------------");

        var result = sb.ToString();
        return result;
    }

    public override string ToString()
    {
        return $"Salves: {Slaves.Count}";
    }
}
