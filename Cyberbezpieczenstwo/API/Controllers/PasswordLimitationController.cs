using System;
using Cyberbezpieczenstwo.API.Modelsp;
using Cyberbezpieczenstwo.Data.Models;
using Cyberbezpieczenstwo.Data.Specifications;
using Cyberbezpieczenstwo.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cyberbezpieczenstwo.API.Controllers;

public class PasswordLimitationController : BaseApiController
{
	private readonly IRepository<PasswordLimitation> _repository;

	public PasswordLimitationController(IRepository<PasswordLimitation> repository)
	{
		_repository = repository;
	}

	[HttpGet]
	public async Task<IEnumerable<PasswordLimitationDTO>> List()
	{
		var limitationsDTOs = (await _repository.ListAsync())
			.Select(limitation => new PasswordLimitationDTO
			(
				id: limitation.Id,
				limitationName: limitation.LimitationName,
				isActive: limitation.IsActive
                )).ToList();
		return limitationsDTOs;
	}

	[HttpPut]
	public async Task<PasswordLimitationDTO> Update(PasswordLimitationDTO request)
	{
		var limitationSpec = new PasswordLimitationById(request.Id);
		var limitationToUpdate = await _repository.FirstOrDefaultAsync(limitationSpec);

		limitationToUpdate.IsActive = request.IsActive;

		await _repository.UpdateAsync(limitationToUpdate);

		return new PasswordLimitationDTO
			(
			limitationToUpdate.Id,
			limitationToUpdate.LimitationName,
			limitationToUpdate.IsActive);

    }
}

