//This file defines the data fields for the ticket obj
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ModelsLayer;

public class Ticket
{
    public string ?Guide { get; set; }
    public int UserId { get; set; }
    public string ?Reason { get; set; }
    public int Amount { get; set; }
    public string ?Description { get; set; }

    //0 = pending, 1 = approved, 2 = rejected
    public int Status { get; set; }

    public Ticket(string guide, string reason, int amount, string description, int statusId, int userId)
    {
        this.Guide = guide;
        this.Reason = reason;
        this.Amount = amount;
        this.Description = description;
        this.Status = statusId;
        this.UserId = userId;
    }

    public Ticket() {}
}