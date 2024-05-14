namespace LoadBalancer;
public class Instance
{
    public bool IsBusy { get; set; }

    public async Task ExecuteTask(Work work)
    {
        await Task.Delay(work.Time);
    }

    public override string ToString()
    {
        return $"IsBusy: {IsBusy}";
    }
}
