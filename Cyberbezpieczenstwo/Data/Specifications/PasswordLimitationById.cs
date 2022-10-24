using System;
using Ardalis.Specification;
using Cyberbezpieczenstwo.Data.Models;

namespace Cyberbezpieczenstwo.Data.Specifications;

public class PasswordLimitationById : Specification<PasswordLimitation>
{
	public PasswordLimitationById(int id)
	{
		Query
			.Where(limitation => limitation.Id == id);
	}
}

