using System;
namespace Cyberbezpieczenstwo.API.Models;

public class UserDTO : CreateUserDTO
{
	public UserDTO(int id, string login, string password, int role, DateTime? expirationTime, bool isBlocked, ICollection<UsedPasswordDTO> usedPasswords, bool hasOneUsePassword, string? oneUsePassword,int? x) : base(login, password, role, expirationTime, isBlocked, usedPasswords, hasOneUsePassword, oneUsePassword,x)
	{
		Id = id;
	}

	public UserDTO()
	{

	}

	public int Id { get; set; }
}

public abstract class CreateUserDTO
{
	protected CreateUserDTO(string login, string password, int role, DateTime? expirationTime, bool isBlocked, ICollection<UsedPasswordDTO> usedPasswords,bool hasOneUsePassword,string? oneUsePassword,int? x)
	{
		Login = login;
		Password = password;
		Role = role;
		ExpirationTime = expirationTime;
		IsBlocked = isBlocked;
		UsedPasswords = usedPasswords;
		HasOneUsePassword = hasOneUsePassword;
		OneUsePassword = oneUsePassword;
		X = x;

    }

	protected CreateUserDTO()
	{

	}

	public string Login { get; set; }
	public string Password { get; set; }
	public int Role { get; set; }
	public DateTime? ExpirationTime { get; set; }
	public bool IsBlocked { get; set; }
	public ICollection<UsedPasswordDTO> UsedPasswords { get; set; }
    public bool HasOneUsePassword { get; set; }
    public string? OneUsePassword { get; set; }
	public int? X { get; set; }
}

