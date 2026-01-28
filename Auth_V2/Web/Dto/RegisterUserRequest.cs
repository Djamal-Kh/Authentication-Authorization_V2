namespace Web.Dto;

public record RegisterUserRequest(Guid Id, string Username, string Email, string Password);
