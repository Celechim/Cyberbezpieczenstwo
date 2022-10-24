using System;
namespace Cyberbezpieczenstwo.API.Modelsp;

public class PasswordLimitationDTO : CreatePasswordDTO
{
	public PasswordLimitationDTO(int id, string limitationName, bool isActive) : base(limitationName, isActive)
	{
		Id = id;
	}

	public int Id { get; set; }
}

public abstract class CreatePasswordDTO
{
	public CreatePasswordDTO(string limitationName, bool isActive)
	{
		LimitationName = limitationName;
		IsActive = isActive;
	}

	public string LimitationName { get; set; }
	public bool IsActive { get; set; }
}

