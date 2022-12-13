using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer;
using BusinessLayer;

namespace ApiUi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    // Dependency injection for ticket service class
    private readonly ITicketService _its;
    public TicketController(ITicketService its) => this._its = its;

    [HttpPost("Ticket")]
    public ActionResult<Ticket> Ticket(int userId, string reason, int amount, string description)
    {
        Ticket ticket = _its.AddTicket(userId, reason, amount, description);
        return Created("path/", ticket);
    }

    [HttpGet("PendingTickets")]
    public ActionResult<List<Ticket>> PendingTickets(int managerId)
    {
        List<Ticket> pendingTickets = _its.GetPendingTickets(managerId);
        return Created("path/", pendingTickets);
    }

    [HttpPut("Approve")]
    public ActionResult<Ticket> Approve(int managerId, string ticketId)
    {
        Ticket approvedTicket = _its.ApproveTicket(managerId, ticketId);
        return Created("path/", approvedTicket);
    }

    [HttpPut("Deny")]
    public ActionResult<Ticket> Deny(int managerId, string ticketId) {
        Ticket deniedTicket = _its.DenyTicket(managerId, ticketId);
        return Created("path/", deniedTicket);
    }
}