using LoadBalancer;

namespace UnitTests;

public class AssignWorkToAvailableSlave
{
    public Master Master { get; set; }
    public Slave Slave1 { get; set; }
    public Slave Slave2 { get; set; }
    public Slave Slave3 { get; set; }

    [SetUp]
    public void Setup()
    {
        TestContext.WriteLine(nameof(AssignWorkToAvailableSlave));

        Master = new Master(nbrSlaves: 3, nbrInstances: 3);

        Slave1 = Master.Slaves[0];
        Slave2 = Master.Slaves[1];
        Slave3 = Master.Slaves[2];

        for (var i = 0; i < 3; i++)
        {
            Slave1.AssignTask(new Work(TimeSpan.FromMinutes(5)));
            Slave2.AssignTask(new Work(TimeSpan.FromMinutes(3)));
        }

        TestContext.WriteLine($"Simulate all instances of slave1 and slave2 being busy:\r\n{Master.DisplaySlavesUsage()}");

        TestContext.WriteLine("Add a new task");
        Master.ReceiveTask(new Work(TimeSpan.FromMinutes(10)));

        TestContext.WriteLine(Master.DisplaySlavesUsage());
    }

    [Test]
    public void Should_Assign_Work_To_Available_Slave()
    {
        // The new task should be assigned to slave3, so slave3 should have 2 available instances
        Assert.AreEqual(2, Slave3.GetAvailableInstanceCount());
    }
}