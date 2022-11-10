using System;
using Cyberbezpieczenstwo.API.Models;
using Cyberbezpieczenstwo.Data.Models;
using Cyberbezpieczenstwo.Data.Specifications;
using Cyberbezpieczenstwo.Pages;
using Cyberbezpieczenstwo.Shared;
using Cyberbezpieczenstwo.SharedKernel.Enums;
using Cyberbezpieczenstwo.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cyberbezpieczenstwo.API.Controllers;

public class UserController : BaseApiController
{
	private readonly IRepository<User> _repository;

	public UserController(IRepository<User> repository)
	{
		_repository = repository;
	}

	[HttpGet]
	public async Task<IEnumerable<UserDTO>> List()
	{
		var userDTOs = (await _repository.ListAsync())
			.Select(user => new UserDTO
			(
				id: user.Id,
				login: user.Login,
				password: user.Password,
				role: (int)user.Role,
				expirationTime: user.ExpirationTime,
				isBlocked: user.IsBlocked,
				usedPasswords: user.UsedPasswords
					.Select(usedPassword => new UsedPasswordDTO
					(
						id: usedPassword.Id,
						user: new UserDTO(),
						usedPassword: usedPassword.Password
					)).ToList(),
                hasOneUsePassword:user.HasOneUsePassword,
				oneUsePassword:user.OneUsePassword,
                x: user.X
            ));
		return userDTOs;
	}

	[HttpGet("{login}")]
	public async Task<UserDTO> GetByLogin(string login)
	{
		var userSpec = new UserByLogin(login);
		var user = await _repository.FirstOrDefaultAsync(userSpec);

		if (user == null)
			return null;

		var userDTO = new UserDTO
		(
			id: user.Id,
			login: user.Login,
			password: user.Password,
			role: (int)user.Role,
			expirationTime: user.ExpirationTime,
			isBlocked: user.IsBlocked,
            usedPasswords: user.UsedPasswords
                .Select(usedPassword => new UsedPasswordDTO
                (
                    id: usedPassword.Id,
                    user: new UserDTO(),
                    usedPassword: usedPassword.Password
                )).ToList(),
            hasOneUsePassword: user.HasOneUsePassword,
                oneUsePassword: user.OneUsePassword,
                x: user.X
        );

		return userDTO;
	}

	[HttpGet("/ById/{id}")]
    public async Task<UserDTO> GetById(int id)
    {
        var userSpec = new UserById(id);
        var user = await _repository.FirstOrDefaultAsync(userSpec);

        var userDTO = new UserDTO
        (
            id: user.Id,
            login: user.Login,
            password: user.Password,
            role: (int)user.Role,
            expirationTime: user.ExpirationTime,
			isBlocked: user.IsBlocked,
            usedPasswords: user.UsedPasswords
                .Select(usedPassword => new UsedPasswordDTO
                (
                    id: usedPassword.Id,
                    user: new UserDTO(),
                    usedPassword: usedPassword.Password
                )).ToList(),
            hasOneUsePassword: user.HasOneUsePassword,
                oneUsePassword: user.OneUsePassword,
                x: user.X
        );

        return userDTO;
    }

    [HttpPost]
	public async Task<UserDTO> Post(UserDTO request)
	{
		var newUser = new User
		(
			request.Login,
			request.Password,
			(UserRole)request.Role,
			request.ExpirationTime,
			request.IsBlocked,
			new List<UsedPassword>(),
			request.HasOneUsePassword,
			request.OneUsePassword,
			request.X
        );

		var createdUser = await _repository.AddAsync(newUser);
		createdUser.UsedPasswords.Add(new UsedPassword(createdUser.Id, createdUser.Password));
		await _repository.UpdateAsync(createdUser);

		var result = new UserDTO
		(
			id: createdUser.Id,
			login: createdUser.Login,
			password: createdUser.Password,
			role: (int)createdUser.Role,
			expirationTime: createdUser.ExpirationTime,
			isBlocked: createdUser.IsBlocked,
            usedPasswords: createdUser.UsedPasswords
                .Select(usedPassword => new UsedPasswordDTO
                (
                    id: usedPassword.Id,
                    user: new UserDTO(),
                    usedPassword: usedPassword.Password
                )).ToList(),
            request.HasOneUsePassword,
            request.OneUsePassword,
			request.X
        );

		return result;
	}

	[HttpPut]
	public async Task<UserDTO> Update(UserDTO request)
	{
		var specUser = new UserById(request.Id);
		var userToUpdate = await _repository.FirstOrDefaultAsync(specUser);

		userToUpdate.Login = request.Login;
		if (userToUpdate.Password != request.Password)
			userToUpdate.UsedPasswords.Add(new UsedPassword(userToUpdate.Id, request.Password));
		userToUpdate.Password = request.Password;
		userToUpdate.ExpirationTime = request.ExpirationTime;
		userToUpdate.IsBlocked = request.IsBlocked;
		userToUpdate.HasOneUsePassword = request.HasOneUsePassword;
		userToUpdate.OneUsePassword = request.OneUsePassword;
		userToUpdate.X = request.X;
		userToUpdate.Role = (UserRole)request.Role;

		await _repository.UpdateAsync(userToUpdate);
		return new UserDTO
		(
			id: userToUpdate.Id,
			login: userToUpdate.Login,
			password: userToUpdate.Password,
			role: (int)userToUpdate.Role,
			expirationTime: userToUpdate.ExpirationTime,
			isBlocked: userToUpdate.IsBlocked,
            usedPasswords: userToUpdate.UsedPasswords
                .Select(usedPassword => new UsedPasswordDTO
                (
                    id: usedPassword.Id,
                    user: new UserDTO(),
                    usedPassword: usedPassword.Password
                )).ToList(),
            request.HasOneUsePassword,
            request.OneUsePassword,
			request.X
        );

	}

	//[HttpPatch]
	//public async Task<UserDTO> PatchLogin(UserDTO request)
	//{
	//       var specUser = new UserById(request.Id);
	//       var userToUpdate = await _repository.FirstOrDefaultAsync(specUser);

	//       if (userToUpdate != null)
	//       {
	//		request.ApplyTo

	//       }
	//   }

	[HttpDelete]
	public async Task<IActionResult> Delete(int id)
	{
		var userSpec = new UserById(id);
		var userToDelete = await _repository.FirstOrDefaultAsync(userSpec);

		await _repository.DeleteAsync(userToDelete);

		return Ok(userToDelete);
	}
}

