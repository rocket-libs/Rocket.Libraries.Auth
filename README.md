
# Rocket.Libraries.Auth
An bare-bones dotnet wrapper around the excellent [JWT](https://www.nuget.org/packages/JWT/) library, to simplify encoding and decoding of tokens.

# Quick Start
## 1. Installation
Install the [package from nuget](https://www.nuget.org/packages/Rocket.Libraries.Auth) 

## 2. Configure your dotnet app
1. Include in your ***startup.cs*** file.
	`using  Rocket.Libraries.Auth;`
2. Implement the ***IRocketJwtSecretProvider*** interface.
```csharp
public  class  LatticeSecretProvider : IRocketJwtSecretProvider`
{
	/// <summary>
	/// All this method requires to do is return a string to be used as the	secret
	/// during encoding and decoding.
	/// </summary>
	/// <returns></returns>
	public  Task<string> GetSecretAsync()
	{
		return  Task.FromResult("something extremely secret");
	}
}
```

3. Register services auth library in your ***ConfigureServices*** method.
```csharp
public  void  ConfigureServices (IServiceCollection services)
{
	services.SetupRocketJwtAuth<LatticeSecretProvider>();
	//Register other services
}
```
## 3. Encoding and Decoding Tokens
After installation and configuration as detailed above, you can now encode and decode tokens as show in the example class below.

```csharp
public  class  TokenManager
{
	private  readonly  IRocketJwtIssuer rocketJwtIssuer;
	private  readonly  IRocketJwtTokenDecoder rocketJwtTokenDecoder;
	private  readonly  IMySystemAuthenticator mySystemAuthenticator;
	  

		public  TokenManager(
			IRocketJwtIssuer rocketJwtIssuer, //Provided Rocket.Library.Auth works out of box
		IRocketJwtTokenDecoder rocketJwtTokenDecoder, //Provided Rocket.Library.Auth works out of box
		IMySystemAuthenticator mySystemAuthenticator)
	{
		this.rocketJwtIssuer  =  rocketJwtIssuer;
		this.rocketJwtTokenDecoder  =  rocketJwtTokenDecoder;
		this.mySystemAuthenticator  =  mySystemAuthenticator;
	}

  
  

	public  async  Task  GetClaimsAsync(string token)
	{
		// Below call returns claims dictionary
		// Also verifies signature.
		var claims =  await  rocketJwtTokenDecoder.DecodeTokenAsync(token);
	}

	  

	public  async  Task<string> GetTokenAsync()
	{
		// Sign into your system and return an object of type
		// Rocket.Libraries.Auth.NativeAuthenticationResult
		var nativeAuthenticationResult =  await  mySystemAuthenticator.SignInAsync();

		//Flag whether authentication succeeded or not.
		nativeAuthenticationResult.Authenticated  =  true;

		// Add your claims (IDictionary<string,object>)
		nativeAuthenticationResult.Claims.Add("role", "admin");
		nativeAuthenticationResult.Claims.Add("userId", 23);
	  
		// Set the lifetime of the token in seconds
		nativeAuthenticationResult.LifetimeSeconds  =  3600;
	  
		// Call the token issuer
		return  await  rocketJwtIssuer.GetTokenAsync(nativeAuthenticationResult);
	}

}
```


	

    
	
