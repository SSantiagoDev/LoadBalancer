using LoadBalancer;

namespace UnitTests;

public class AllSlavesAreBusy
{
    public Master Master { get; set; }
    public Slave Slave1 { get; set; }
    public Slave Slave2 { get; set; }
    public Slave Slave3 { get; set; }

    [SetUp]
    public void Setup()
    {
        TestContext.WriteLine(nameof(AllSlavesAreBusy));

        Master = new Master(nbrSlaves: 3, nbrInstances: 3);

        Slave1 = Master.Slaves[0];
        Slave2 = Master.Slaves[1];
        Slave3 = Master.Slaves[2];

        for (var i = 0; i < 3; i++)
        {
            Slave1.AssignTask(new Work(TimeSpan.FromMinutes(5)));
            Slave2.AssignTask(new Work(TimeSpan.FromMinutes(5)));
            Slave3.AssignTask(new Work(TimeSpan.FromMinutes(5)));
        }

        TestContext.WriteLine($"Simulate all instances of all slaves being busy:\r\n{Master.DisplaySlavesUsage()}");

        TestContext.WriteLine("Add a new task");
        Master.ReceiveTask(new Work(TimeSpan.FromMinutes(5)));

        TestContext.WriteLine(Master.DisplaySlavesUsage());
    }

    [Test]
    public void Should_Add_Work_To_Queue()
    {
        // The new task should be added to the queue
        Assert.AreEqual(1, Master.GetQueueSize());
    }
}