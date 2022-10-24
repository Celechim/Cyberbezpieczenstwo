using System;
using Ardalis.Specification;
using Cyberbezpieczenstwo.Data.Models;

namespace Cyberbezpieczenstwo.Data.Specifications;

public class UserById : Specification<User>
{
	public UserById(int id)
	{
		Query
			.Where(user => user.Id == id);
	}
}

