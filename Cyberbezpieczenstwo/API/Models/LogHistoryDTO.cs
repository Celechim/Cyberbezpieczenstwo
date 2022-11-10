using System;
namespace Cyberbezpieczenstwo.API.Modelsp;

public class LogHistoryDTO : CreateLogHistoryDTO
{
    public LogHistoryDTO(int id, string login, string desc,bool status) : base(login, desc, status)
    {
        Id = id;
    }
    public LogHistoryDTO() { }

    public int Id { get; set; }
}

public abstract class CreateLogHistoryDTO
{
    public CreateLogHistoryDTO(string login, string desc, bool status)
    {
        UserName = login;
        Date = DateTime.Now;
        Desc = desc;
        Status = status;
    }
    public CreateLogHistoryDTO() { }
    public string UserName { get; set; }
    public DateTime Date { get; set; }
    public string Desc { get; set; }
    public bool Status { get; set; }
}

