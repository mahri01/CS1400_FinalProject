namespace TestProject;
using NUnit.Framework;
using Customer;
public class Tests
{
    static string memberShipFile = "C:\\Users\\mahri.gurbanova\\Shopping_Project\\Proj\\TestProject\\membershipTest.txt";
    static string customerCartFile = "C:\\Users\\mahri.gurbanova\\Shopping_Project\\Proj\\TestProject\\customerCartTest.txt";
    static string paymentInfoFile = "C:\\Users\\mahri.gurbanova\\Shopping_Project\\Proj\\TestProject\\PaymentInfoTest.txt";

    [SetUp]
    public void Setup()
    {
        CustomerProfile.membershipFile = memberShipFile;
        CustomerProfile.customerCartFile = customerCartFile;
        CustomerProfile.paymentInfoFile = paymentInfoFile;
    }

    [TearDown]
    public void TearDown()
    {
        File.WriteAllText(memberShipFile, string.Empty);
        File.WriteAllText(customerCartFile, string.Empty);
        File.WriteAllText(paymentInfoFile, string.Empty);
    }

    [Test]
    public void signUpNewMember()
    {
        var userName = "testUser";
        var password = "test";

        CustomerProfile.WriteUserToFile(userName, password);

        var fileContent = File.ReadAllText(memberShipFile);

        Assert.IsTrue(fileContent.Contains("testUser"));
    }

    [Test]
    public void checkIfNewMemberCanSignin()
    {
        var userName = "testUser";
        var password = "test";

        CustomerProfile.WriteUserToFile(userName, password);

        var result = CustomerProfile.SignInUser(userName, password);

        Assert.IsTrue(result);
    }

    [Test]
    public void signInBadUser()
    {
        var userName = "badUser";
        var password = "test";

        var result = CustomerProfile.SignInUser(userName, password);

        Assert.IsFalse(result);
    }

    [Test]
    public void checkIfUserExists()
    {
        var userName = "Ayden";
        var password = "Bash";

        CustomerProfile.WriteUserToFile(userName, password);

        Assert.Throws<Exception>(() => CustomerProfile.WriteUserToFile(userName, password));
    }

    [Test]
    public void checkIfUserExistsInTheFile()
    {
        var userName = "Maren";
        var password = "Bench";

        CustomerProfile.WriteUserToFile(userName, password);

        Assert.Throws<Exception>(() => CustomerProfile.WriteUserToFile(userName, password));
    }

    [Test]
    public void AddToCart()
    {
        var userName = "test3";
        var password = "hello";

        CustomerProfile.WriteUserToFile(userName, password);

        var result = CustomerProfile.SignInUser(userName, password);

        Assert.IsTrue(result);

        CustomerProfile.AddToCart("apples", 2, "lb", 2.99);

        var fileContent = File.ReadAllText(customerCartFile);

        Assert.IsTrue(fileContent.Contains("apples"));
    }

    [Test]
    public void AddToCartExistingItem()
    {
        var userName = "test3";
        var password = "hello";

        CustomerProfile.WriteUserToFile(userName, password);

        var result = CustomerProfile.SignInUser(userName, password);

        Assert.IsTrue(result);

        CustomerProfile.AddToCart("apples", 2, "lb", 2.99);

        CustomerProfile.AddToCart("apples", 2, "lb", 2.99);

        var fileContent = File.ReadAllText(customerCartFile);

        Assert.IsTrue(fileContent.Contains("4lb"));
    }

    [Test]
    public void PaymentInfoCash()
    {
        CustomerProfile.CustomerInfo.Name = "John";
        var totalPrice = 10.00;
        var paymentType = "cash";

        CustomerProfile.PaymentToFile(totalPrice, paymentType);

        var fileContent = File.ReadAllText(paymentInfoFile);

        Assert.IsTrue(fileContent.Contains("John"));
        Assert.IsTrue(fileContent.Contains("cash"));
    }
    [Test]
    public void PaymentInfoCard()
    {
        CustomerProfile.CustomerInfo.Name = "John";
        var totalPrice = 10.00;
        var paymentType = "card";

        CustomerProfile.PaymentToFile(totalPrice, paymentType);

        var fileContent = File.ReadAllText(paymentInfoFile);

        Assert.IsTrue(fileContent.Contains("John"));
        Assert.IsTrue(fileContent.Contains("card"));
    }
}