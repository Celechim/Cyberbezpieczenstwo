using System;
using Cyberbezpieczenstwo.Data.Models;

namespace Cyberbezpieczenstwo.Data.Interfaces;

public interface IUserService
{
	Task<User> GetUserByLogin(string login);
	Task<IEnumerable<User>> GetAllUsers();
}

