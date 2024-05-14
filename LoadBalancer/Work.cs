namespace LoadBalancer;
public class Work
{
    public TimeSpan Time { get; set; }

    public Work(TimeSpan time)
    {
        Time = time;
    }

    public override string ToString()
    {
        return $"Work time: {Time.TotalSeconds} sec";
    }
}
