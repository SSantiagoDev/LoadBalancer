using System.Text;

namespace LoadBalancer;
public class Slave
{
    private readonly Master _master;
    private readonly List<Instance> _instances;

    public Slave(Master master, int nbrOfInstances)
    {
        _master = master;
        _instances = new List<Instance>();

        for (var i = 0; i < nbrOfInstances; i++)
            _instances.Add(new Instance());
    }

    public async Task AssignTask(Work work)
    {
        var availableInstance = _instances.FirstOrDefault(i => !i.IsBusy);

        if (availableInstance != null)
        {
            availableInstance.IsBusy = true;
            await availableInstance.ExecuteTask(work);
            availableInstance.IsBusy = false;
            _master.NotifyTaskCompleted();
        }
        else
            _master.AddTaskToQueue(work);
    }

    public bool HasAvailableInstance()
    {
        return _instances.Any(i => !i.IsBusy);
    }

    public double GetAvailableInstanceCount()
    {
        return _instances.Count(i => !i.IsBusy);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        for (var i = 0; i < _instances.Count; i++)
        {
            sb.AppendLine($"Instance {i + 1}: {_instances[i].ToString()}");
        }

        var result = sb.ToString();

        return result;
    }
}
