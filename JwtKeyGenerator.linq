<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	JwtKeyGenerator.GenerateKey().Dump();
}

public class JwtKeyGenerator
{
	public static string GenerateKey()
	{
		//Has to be 256 bits to be jwt.io compliant - using the HS256 algorithm
		// https://www.jwt.io/ to do a complete test you need:
		// 1. A generated JWT
		// 2. The key used to generate that JWT
		var key = new byte[256];
		using (var rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(key);
		}
		return Convert.ToBase64String(key);
	}
}
