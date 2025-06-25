using System;

namespace API.Helpers.JsonModels;

public class MessageParams : PageParams
{
    public string? UserName { get; set; }
    public int UserID { get; set; }
    public string? Container { get; set; }
}
