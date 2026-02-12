namespace EcommerceApi.Errors;

public class NotFoundException(string message) : Exception(message) { }
