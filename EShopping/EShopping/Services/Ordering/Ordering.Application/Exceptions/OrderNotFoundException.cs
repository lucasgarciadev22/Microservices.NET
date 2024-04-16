namespace Ordering.Application.Exceptions;

public class OrderNotFoundException(string name, object key)
    : ApplicationException($"Order {name} - {key} is not found.") { }
