using System;
using Cyberbezpieczenstwo.SharedKernel;

namespace Cyberbezpieczenstwo.Data.Models;

public class UsedPassword : EntityBase
{
	public virtual User User { get; set; }
	public int UserId { get; set; }
	public string Password { get; set; }

	public UsedPassword()
	{
	}

	public UsedPassword(int userId, string password)
	{
		UserId = userId;
		Password = password;
	}
}

