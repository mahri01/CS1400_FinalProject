namespace TestProject;
using NUnit.Framework;
using Customer;
public class Tests
{
    static string memberShipFile = "C:\\Users\\mahri.gurbanova\\Shopping_Project\\Proj\\TestProject\\membershipTest.txt";
    static string customerCartFile = "C:\\Users\\mahri.gurbanova\\Shopping_Project\\Proj\\TestProject\\customerCartTest.txt";
    [SetUp]
    public void Setup()
    {
        CustomerProfile.membershipFile = memberShipFile;
        CustomerProfile.customerCartFile = customerCartFile;
    }

    [Test]
    public void signUpNewMember()
    {
        var userName = "test";
        var password = "test";

        CustomerProfile.WriteUserTheFile(userName, password);

        //now call signInMethod
        var result = CustomerProfile.SignInUser(userName, password);

        Assert.IsTrue(result);

        //clear the file
        File.WriteAllText(memberShipFile, string.Empty);
    }
    [Test]
    public void signInMember()
    {
        var userName = "bad";
        var password = "user";

        //now call signInMethod
        var result = CustomerProfile.SignInUser(userName, password);

        Assert.IsFalse(result);

        //clear the file
        File.WriteAllText(memberShipFile, string.Empty);
    }

    [Test]
    public void checkIfUserExists()
    {
        var userName = "test2";
        var password = "hello";

        CustomerProfile.WriteUserTheFile(userName, password);

        //check for exception
        Assert.Throws<Exception>(() => CustomerProfile.WriteUserTheFile(userName, password));

        //clear the file
        File.WriteAllText(memberShipFile, string.Empty);
    }

    [Test]
    public void AddToCart()
    {
        var userName = "test3";
        var password = "hello";

        CustomerProfile.WriteUserTheFile(userName, password);

        //now call signInMethod
        var result = CustomerProfile.SignInUser(userName, password);

        Assert.IsTrue(result);

        //add to cart
        CustomerProfile.AddToCart("apples", 2, "lb", 2.99);

        //check if the file is empty
        var fileContent = File.ReadAllText(customerCartFile);

        //test if the item name in the cart is apples
        Assert.IsTrue(fileContent.Contains("apples"));

    }

    [Test]
    public void ()
    {

    }
}