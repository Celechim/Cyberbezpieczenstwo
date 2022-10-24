using System;
namespace Cyberbezpieczenstwo.API.Models;

public class UserDTO : CreateUserDTO
{
	public UserDTO(int id, string login, string password, int role, DateTime? expirationTime, bool isBlocked, ICollection<UsedPasswordDTO> usedPasswords) : base(login, password, role, expirationTime, isBlocked, usedPasswords)
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
	protected CreateUserDTO(string login, string password, int role, DateTime? expirationTime, bool isBlocked, ICollection<UsedPasswordDTO> usedPasswords)
	{
		Login = login;
		Password = password;
		Role = role;
		ExpirationTime = expirationTime;
		IsBlocked = isBlocked;
		UsedPasswords = usedPasswords;
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
}

