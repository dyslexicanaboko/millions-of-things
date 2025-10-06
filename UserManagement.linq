<Query Kind="Program">
  <Connection>
    <ID>203a5e2a-fb01-4ef4-9934-763dddf526f6</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <Database>InStock</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
  <NuGetReference>BCrypt.Net-Next</NuGetReference>
  <Namespace>BCrypt.Net</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	//These are intended for testing, hence why they are in plain text
	GetUpdatePasswordStatement(1, "emmC2YNvh%9LtNMHWo#T").Dump();
	GetUpdatePasswordStatement(2, "6Uh@16n%jLZKOXZO").Dump();
}

public static string GetUpdatePasswordStatement(int userId, string plainTextPassword)
{
	var pw = HashPassword(plainTextPassword);

	return $"update public.user set password = '{pw}' where user_id = {userId}";
}

private static string HashPassword(string plainTextPassword)
{
	var salt = BCrypt.Net.BCrypt.GenerateSalt(12);

	var hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainTextPassword, salt);

	return hashedPassword;
}