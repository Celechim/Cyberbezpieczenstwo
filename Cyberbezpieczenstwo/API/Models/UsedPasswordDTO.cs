using System;
namespace Cyberbezpieczenstwo.API.Models;

public class UsedPasswordDTO : CreateUsedPasswordDTO
{
	public UsedPasswordDTO(int id, UserDTO user, string usedPassword) : base(user, usedPassword)
	{
		Id = id;
	}
	public int Id { get; set; }
}

public abstract class CreateUsedPasswordDTO
{
	public CreateUsedPasswordDTO(UserDTO user, string usedPassword)
	{
		User = user;
		UsedPassword = usedPassword;
	}
	public UserDTO User { get; set; }
	public string UsedPassword { get; set; }
}

