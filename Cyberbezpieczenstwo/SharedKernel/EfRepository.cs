using System;
using Ardalis.Specification.EntityFrameworkCore;
using Cyberbezpieczenstwo.Data;
using Cyberbezpieczenstwo.SharedKernel.Interfaces;

namespace Cyberbezpieczenstwo.SharedKernel;

public class EfRepository<T> : RepositoryBase<T>, IRepository<T> where T : class, IAggregateRoot
{
	public EfRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
	}
}

