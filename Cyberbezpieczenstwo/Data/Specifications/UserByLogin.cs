using System;
using Ardalis.Specification;
using Cyberbezpieczenstwo.Data.Models;

namespace Cyberbezpieczenstwo.Data.Specifications;

public class UserByLogin : Specification<User>
{
	public UserByLogin(string login)
	{
		Query
			.Where(user => user.Login == login);
	}
}

