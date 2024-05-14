using LoadBalancer;

namespace UnitTests;

public class AssignWorkToLessBusySlave
{
    public Master Master { get; set; }
    public Slave Slave1 { get; set; }
    public Slave Slave2 { get; set; }
    public Slave Slave3 { get; set; }

    [SetUp]
    public void Setup()
    {
        TestContext.WriteLine(nameof(AssignWorkToLessBusySlave));

        Master = new Master(nbrSlaves: 3, nbrInstances: 3);

        Slave1 = Master.Slaves[0];
        Slave2 = Master.Slaves[1];
        Slave3 = Master.Slaves[2];

        Slave1.AssignTask(new Work(TimeSpan.FromMinutes(5)));
        Slave1.AssignTask(new Work(TimeSpan.FromMinutes(5)));

        Slave2.AssignTask(new Work(TimeSpan.FromMinutes(5)));
        Slave2.AssignTask(new Work(TimeSpan.FromMinutes(5)));
        Slave2.AssignTask(new Work(TimeSpan.FromMinutes(5)));

        Slave3.AssignTask(new Work(TimeSpan.FromMinutes(5)));

        TestContext.WriteLine($"Simulate some instances being busy:\r\n{Master.DisplaySlavesUsage()}");

        TestContext.WriteLine("Add a new task");
        Master.ReceiveTask(new Work(TimeSpan.FromMinutes(5)));

        TestContext.WriteLine(Master.DisplaySlavesUsage());
    }

    [Test]
    public void Should_Assign_Work_To_Less_Busy_Slave()
    {
        // The new task should be added to the less busy slave, which is slave3
        Assert.AreEqual(1, Slave3.GetAvailableInstanceCount());
    }
}