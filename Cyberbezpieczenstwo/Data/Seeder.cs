using System;
using Cyberbezpieczenstwo.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Cyberbezpieczenstwo.Data;

public static class Seeder
{
	public static void Seed(ApplicationDbContext context)
	{
		context.Database.Migrate();

		context.SaveChanges();

		/*if (!context.PasswordLimitations.Where(x => x.LimitationName == "Co najmniej 8 znaków").Any())
		{
			var newLimitation = new PasswordLimitation()
			{
				LimitationName = "Co najmniej 8 znaków",
				IsActive = false
			};
			context.Add(newLimitation);
			context.SaveChanges();
		}*/
	}
}

