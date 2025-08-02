namespace ContactsApi.Controllers;

using Microsoft.AspNetCore.Mvc;


public class ContactsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetContacts()
    {
        // Logic to retrieve contacts would go here
        return Ok(new { Message = "List of contacts" });
    }

    [HttpPost]
    public IActionResult AddContact([FromBody] string contact)
    {
        // Logic to add a new contact would go here
        return CreatedAtAction(nameof(GetContacts), new { Message = "Contact added successfully" });
    }
}

