using System;
using Ardalis.Specification;

namespace Cyberbezpieczenstwo.SharedKernel.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}

