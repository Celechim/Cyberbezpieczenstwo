using Cyberbezpieczenstwo.API.Models;
using Cyberbezpieczenstwo.API.Modelsp;
using Cyberbezpieczenstwo.Data.Models;
using Cyberbezpieczenstwo.Data.Specifications;
using Cyberbezpieczenstwo.Pages;
using Cyberbezpieczenstwo.SharedKernel.Enums;
using Cyberbezpieczenstwo.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Cyberbezpieczenstwo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogHistoryController : BaseApiController
    {
        private readonly IRepository<LogHistory> _repository;
        private readonly IRepository<SecuritySettings> _security;

        public LogHistoryController(IRepository<LogHistory> repository, IRepository<SecuritySettings> security)
        {
            _repository = repository;
            _security = security;   
        }

        [HttpGet]
        public async Task<IEnumerable<LogHistory>> List()
        {
            var logHistoryList = (await _repository.ListAsync())
                .Select(log => new LogHistory
                (
                    id: log.Id,
                    userName: log.UserName,
                    description: log.Description,
                    status: log.Status,
                    date: log.Date

                ));
            return logHistoryList;
        }
        [HttpGet]
        [Route("get1")]
        public async Task<DateTime> DateOfLastFail(string name)
        {
            var a = new SecurityById(0);
            var c = await _security.FirstOrDefaultAsync(a);
            if (!c.IsSetLimitOfMaxFailedLoginAttemps)
                return DateTime.UtcNow;

                bool isBlocked = false;
            var logHistoryList = (await _repository.ListAsync()).Where(
                x => x.UserName == name 
                && DateTime.UtcNow.Subtract(x.Date).TotalMinutes< 15 
                && x.Status == false).Select(x=>x.Date).ToList();
            DateTime lastFail=logHistoryList.LastOrDefault();

            
            if (logHistoryList.Count() > c.MaxNumbersOfFailedLoginAttemps)
                return lastFail.AddMinutes(15);
            else
                return DateTime.UtcNow;
        }
        [HttpPost]
        public async Task Post(LogHistoryDTO request)
        {
            var newRecord = new LogHistory
            (
                id:request.Id,
                userName: request.UserName,
                date: DateTime.UtcNow,
                description: request.Desc,
                status:request.Status
            );

            await _repository.AddAsync(newRecord);

        }
    }
}
