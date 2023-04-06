namespace Tests;

using Opalenica.UI;

using System.Text.RegularExpressions;

public class Tests
{
    [Test]
    [TestOf(nameof(Grid.SizeRegex))]
    public void Grid_SizeRegex()
    {
        Assert.IsTrue(Regex.IsMatch("1x1,1x1", Grid.SizeRegex));
        Assert.IsTrue(Regex.IsMatch("10x10,10x10", Grid.SizeRegex));
        Assert.IsTrue(Regex.IsMatch("1x100,100x100", Grid.SizeRegex));
        Assert.IsTrue(Regex.IsMatch("1000x1000,1000x1000", Grid.SizeRegex));
        Assert.IsTrue(Regex.IsMatch("10000x10000,10000x10000", Grid.SizeRegex));
        Assert.IsTrue(Regex.IsMatch("100000x100000,1x1", Grid.SizeRegex));
        Assert.IsFalse(Regex.IsMatch("0x0,0x0", Grid.SizeRegex));
        Assert.IsFalse(Regex.IsMatch("0x0,1x1", Grid.SizeRegex));
        Assert.IsFalse(Regex.IsMatch("1x1,0x0", Grid.SizeRegex));
        Assert.IsFalse(Regex.IsMatch("1x1,1x1,1x1", Grid.SizeRegex));
        Assert.IsFalse(Regex.IsMatch("1x0,1x1", Grid.SizeRegex));
    }
}