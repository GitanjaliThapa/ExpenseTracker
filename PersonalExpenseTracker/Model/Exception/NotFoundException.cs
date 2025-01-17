namespace PersonalExpenseTracker.Model.Exception;



    public class NotFoundException(string message) : IOException(message);
