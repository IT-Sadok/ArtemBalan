using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MyFarm.Data;

public class AuthOptions
{
    public string ISSUER { get; set; }
    public string AUDIENCE { get; set; }
    private string KEY { get; set; }

    public SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}