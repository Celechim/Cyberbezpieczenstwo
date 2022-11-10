using Cyberbezpieczenstwo.SharedKernel;
using Cyberbezpieczenstwo.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cyberbezpieczenstwo.Data.Models
{
    public class LogHistory : EntityBase, IAggregateRoot
    {
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }

        public LogHistory()
        {

        }
        public LogHistory(int id,string userName,DateTime date,string description,bool status)
        {
            Id = id;
            UserName = userName;
            Date = date;
            Description = description;
            Status = status;
        }
    }
}
