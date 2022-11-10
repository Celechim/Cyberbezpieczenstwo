using Ardalis.Specification;
using Cyberbezpieczenstwo.Data.Models;

namespace Cyberbezpieczenstwo.Data.Specifications;

public class SecurityById : Specification<SecuritySettings>
{
	public SecurityById(int id)
	{
		Query
			.Where(setting => setting.Id == id);
	}
}
