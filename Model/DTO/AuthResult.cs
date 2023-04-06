using System;

public class AuthResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Token { get; set; }
    public DateTime? Expiration { get; set; }
    public string[] Errors { get; internal set; }
}
