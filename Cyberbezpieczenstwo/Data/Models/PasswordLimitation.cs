using System;
using Cyberbezpieczenstwo.SharedKernel;
using Cyberbezpieczenstwo.SharedKernel.Interfaces;

namespace Cyberbezpieczenstwo.Data.Models;

public class PasswordLimitation : EntityBase, IAggregateRoot
{
	public string LimitationName { get; set; }
	public bool IsActive { get; set; }

	public PasswordLimitation()
	{
	}
}

