using Cyberbezpieczenstwo.API.Models;
using Cyberbezpieczenstwo.API.Modelsp;
using Cyberbezpieczenstwo.Data.Models;
using Cyberbezpieczenstwo.Data.Specifications;
using Cyberbezpieczenstwo.Pages;
using Cyberbezpieczenstwo.SharedKernel.Enums;
using Cyberbezpieczenstwo.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Cyberbezpieczenstwo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecuritySettingsController : BaseApiController
    {
        private readonly IRepository<SecuritySettings> _repository;

        public SecuritySettingsController(IRepository<SecuritySettings> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<SecuritySettings>>> Get()
        {
            var SecurityList = (await _repository.ListAsync())
                .Select(options => new SecuritySettings
                (
                    id: options.Id,
                    isSetLimitOfMaxFailedLoginAttemps:options.IsSetLimitOfMaxFailedLoginAttemps,
                    maxNumbersOfFailedLoginAttemps:options.MaxNumbersOfFailedLoginAttemps,
                    isSetAutoLogout: options.IsSetAutoLogout,
                    secToAutoLogout: options.SecToAutoLogout,
                    captchaEnabled: options.CaptchaEnabled,
                    reCaptchaEnabled: options.ReCaptchaEnabled
                    
                )).ToList();
            return SecurityList;
        }
        [HttpPut]
        public async Task<IActionResult> Update(SecuritySettingsDTO request)
        {
            var query = new SecurityById(request.Id);
            var securityToUpdate = await _repository.FirstOrDefaultAsync(query);
            securityToUpdate.IsSetLimitOfMaxFailedLoginAttemps = request.IsSetLimitOfMaxFailedLoginAttemps;
            securityToUpdate.MaxNumbersOfFailedLoginAttemps = request.MaxNumbersOfFailedLoginAttemps;
            securityToUpdate.IsSetAutoLogout = request.IsSetAutoLogout;
            securityToUpdate.SecToAutoLogout = request.SecToAutoLogout;
            securityToUpdate.CaptchaEnabled = request.CaptchaEnabled;
            securityToUpdate.ReCaptchaEnabled = request.ReCaptchaEnabled;

            await _repository.UpdateAsync(securityToUpdate);
            return Ok();
        }
    }
}
