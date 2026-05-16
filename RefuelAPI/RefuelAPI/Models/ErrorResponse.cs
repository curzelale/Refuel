namespace RefuelAPI.Models;

/// <summary>Standard error response body returned when a request fails.</summary>
/// <param name="Error">Human-readable message describing what went wrong.</param>
public record ErrorResponse(string Error);
