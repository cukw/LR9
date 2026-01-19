using PasswordValidator;
using Xunit;
using System.Web;

namespace PasswordValidatorTest;

public class PasswordValidatorTests
{
    private readonly PasswordValidator.PasswordValidator _validator = new();

    [Fact]
    public void Empty_ReturnsError()
    {
        var result = _validator.CheckPwd(string.Empty);

        Assert.Contains("[EMPTY]:", result);
    }

    [Theory]
    [InlineData("7Symbol","SHORT")]
    [InlineData("UPPER", "LOWERCASE")]
    public void CoolPass(string pwd, string ex_code)
    {
        var res = _validator.CheckPwd(pwd);

        Assert.Contains($"[{ex_code}]:", res );
    }

    [Fact]
    public void GoodPass()
    {
        var res = _validator.CheckPwd("Absd$54%DbsDfsedf");

        Assert.Equal("Password is Good", res);
    }
}
