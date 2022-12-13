using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModelsLayer;
using RepoLayer;
namespace BusinessLayer;

public interface ITicketService
{
    public Ticket AddTicket(int userId, string reason, int amount, string description);
    public List<Ticket> GetPendingTickets(int userId);
    public Ticket ApproveTicket(int userId, string ticketId);
    public Ticket DenyTicket(int userId, string ticketId);
    public List<Ticket> GetUserTickets(int userId);
    public List<Ticket> GetUserTickets(int userId, int status);
}

public class TicketService : ITicketService
{
    private readonly ITicketRepo _itr;
    private readonly IUserRepo _iur;
    private readonly IVerificationServices _ivs;
    private readonly ITicketVerificationServices _itvs;
    public TicketService(ITicketRepo itr, IUserRepo iur)
    {
        this._itr = itr;
        this._iur = iur;
        this._ivs = new VerificationServices(this._iur);
        this._itvs = new TicketVerificationServices(this._itr);
    }

    public Ticket AddTicket(int userId, string reason, int amount, string description)
    {
        if(!_ivs.IsEmployee(userId) || !_itvs.VerifiedTicket(reason, amount, description))
        {
            Console.WriteLine("Invalid userId, or your ticket was invalid.");
            return null!;
        }
        return _itr.SubmitTicket(Guid.NewGuid().ToString(), reason, amount, description, userId);
    }
    public List<Ticket> GetPendingTickets(int managerId)
    {
        if(!_ivs.IsManager(managerId))
        {
            Console.WriteLine("User does not exist or have the right permission");
            return null!;
        }
        return _itr.GetPending(managerId);
    }
    public Ticket ApproveTicket(int userId, string ticketId) {
        if(!_ivs.IsManager(userId) || !_itvs.VerifiedStatusChange(userId, ticketId)){
            Console.WriteLine("Invalid manager Id, manager is trying edit an invalid ticket, or ticket doesn't exist");
            return null!;
        } 
        return _itr.UpdateTicket(ticketId, 1);
    }
    public Ticket DenyTicket(int userId, string ticketId) {
        if(!_ivs.IsManager(userId) || !_itvs.VerifiedStatusChange(userId, ticketId)){
            Console.WriteLine("Invalid manager Id, manager is trying to edit an invalid ticket, or ticket doesn't exist");
            return null!;
        }
        return _itr.UpdateTicket(ticketId, 2);
    }
    public List<Ticket> GetUserTickets(int userId) {
        if(!_ivs.IsEmployee(userId)) {
            Console.WriteLine("Invalid userId");
            return null!;
        }
        return _itr.GetTickets(userId);
    }
    public List<Ticket> GetUserTickets(int userId, int status) {
        if(!_ivs.IsEmployee(userId)) {
            Console.WriteLine("Invalid userId");
            return null!;
        }
        return _itr.GetTickets(userId, status);
    }
}