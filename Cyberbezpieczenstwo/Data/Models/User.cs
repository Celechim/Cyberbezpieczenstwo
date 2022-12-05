using Cyberbezpieczenstwo.SharedKernel;
using Cyberbezpieczenstwo.SharedKernel.Enums;
using Cyberbezpieczenstwo.SharedKernel.Interfaces;

namespace Cyberbezpieczenstwo.Data.Models;

public class User : EntityBase, IAggregateRoot
{
	public string Login { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public UserRole Role { get; set; }
	public DateTime? ExpirationTime { get; set; }
	public bool IsBlocked { get; set; }
	public bool HasOneUsePassword { get; set; }
	public string? OneUsePassword { get; set; }
	public bool Demoware { get; set; }
	public int? X { get; set; }
	public virtual ICollection<UsedPassword>? UsedPasswords { get; set; }

	public User(string login, string password, UserRole role, DateTime? expirationTime, bool isBlocked, ICollection<UsedPassword>? usedPasswords,bool hasOneUsePassword,string oneUsePassword,int? x)
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
		Demoware = true;
    }

	public User()
	{
	}
}

