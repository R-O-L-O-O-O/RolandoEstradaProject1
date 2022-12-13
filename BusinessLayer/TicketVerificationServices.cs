using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModelsLayer;
using RepoLayer;

namespace BusinessLayer;

public interface ITicketVerificationServices
{
    public bool VerifiedReason(string reason);
    public bool VerifiedAmount(int amount);
    public bool VerifiedDescription(string description);
    public bool VerifiedTicket(string reason, int amount, string description);
    public bool IsTicket(string ticketId);
    public bool VerifiedStatusChange(int managerId, string ticketId);
}

public class TicketVerificationServices  : ITicketVerificationServices
{
    private readonly ITicketRepo _itr;
    public TicketVerificationServices(ITicketRepo itr) => this._itr = itr;
    public bool VerifiedTicket(string reason, int amount, string description) => VerifiedReason(reason) && VerifiedAmount(amount) && VerifiedDescription(description);
    public bool VerifiedReason(string reason) => reason.Length > 1;
    public bool VerifiedDescription(string description) => description.Length > 1;
    public bool VerifiedAmount(int amount) => amount > 0 && amount < 10000;

    public bool IsTicket(string ticketId)
    {
        if(_itr.GetTicket(ticketId) is null) return false;
        return true;
    }
    public bool VerifiedStatusChange(int managerId, string ticketId)
    {
        Ticket tmp = _itr.GetTicket(ticketId);
        if(tmp.UserId == managerId || tmp is null || tmp.Status != 0) return false;
        return true;
    }
}