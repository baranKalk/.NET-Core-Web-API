using Authentication.Core.Configuration;
using Authentication.Core.DTOs;
using Authentication.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Services
{
	public interface ITokenService
	{
		TokenDto CreateToken(UserApp userApp);
		ClientTokenDto CreateTokenByClient(Client client);
	}
}
