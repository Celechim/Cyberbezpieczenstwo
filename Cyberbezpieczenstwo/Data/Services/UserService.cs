using System;
using Cyberbezpieczenstwo.Data.Interfaces;
using Cyberbezpieczenstwo.Data.Models;
using Cyberbezpieczenstwo.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cyberbezpieczenstwo.Data.Services;

public class UserService : IUserService
{
	public async Task<User> GetUserByLogin(string login)
	{
		var dbContext = new ApplicationDbContext();

		return await dbContext.CustomUsers.FirstOrDefaultAsync(x => x.Login == login);
	}

	public async Task<IEnumerable<User>> GetAllUsers()
	{
		var dbContext = new ApplicationDbContext();
		dbContext.ChangeTracker.LazyLoadingEnabled = false;
		return await dbContext.CustomUsers.AsNoTracking().ToArrayAsync();
	}
}

