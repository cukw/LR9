using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace PasswordValidator;

public class PasswordValidator
{

    private const string spec_symbol = "!@#$%^&*()-_=+[]{};:'\",.<>/?";

    private static readonly string[] often_pwd =
    {
        "password", 
        "qwerty",
        "admin",
        "123456"
    };

    private record ErrorsMsg(string msg, string error_text);

    public bool CheckPasswordLength(string pwd) => pwd.Length >= 12;

    public bool CheckTitleLetter(string pwd) => pwd.Any(char.IsUpper);

    public bool CheckLowerCaseLetter(string pwd) => pwd.Any(char.IsLower);

    public bool CheckNumInPassword(string pwd) => pwd.Any(char.IsDigit);

    public bool CheckSpecSymbol(string pwd) => pwd.Any(spec_symbol.Contains);

    public bool CheckOftenPass(string pwd) => often_pwd.Contains(pwd);

    public bool CheckStack(string pwd) => Regex.IsMatch(pwd,@"(.)\1{3}");

    public bool CheckWhiteSpace(string pwd) => char.IsWhiteSpace(pwd[0]) && char.IsWhiteSpace(pwd[^1]);


    public string CheckPwd(string? password)
    {
        var _errors = new List<ErrorsMsg>();

        if (string.IsNullOrWhiteSpace(password))
        {
            _errors.Add(new ErrorsMsg("EMPTY", "Password is null/empty/whitespace"));
            return string.Join(Environment.NewLine, _errors.Select(e => $"[{e.msg}]: {e.error_text}"));
        }

        if(!CheckTitleLetter(password))
            _errors.Add(new ErrorsMsg("UPPERCASE", "Password not contains title letter"));
        
        if(!CheckPasswordLength(password))
            _errors.Add(new ErrorsMsg("SHORT", "Password is shortly"));

        if(!CheckLowerCaseLetter(password))
            _errors.Add(new ErrorsMsg("LOWERCASE", "Password not contains lowercase letter"));
        
        if(!CheckNumInPassword(password))
            _errors.Add(new ErrorsMsg("STRING", "Password has only string type"));

        if(!CheckSpecSymbol(password))
            _errors.Add(new ErrorsMsg("SPECIAL", "Password has not special symbol"));
        
        if(CheckOftenPass(password))
            _errors.Add(new ErrorsMsg("OFTEN", "Password is often"));

        if(CheckStack(password))
            _errors.Add(new ErrorsMsg("STACK", "Password has line of 4 letter"));

        if(CheckWhiteSpace(password))
            _errors.Add(new ErrorsMsg("WHITESPACE", "Password has whitespace on start and end of word"));

        return _errors.Count() > 0 ? string.Join(Environment.NewLine, _errors.Select(e=>$"[{e.msg}]:{e.error_text}")) : "Password is Good";
    }



}
