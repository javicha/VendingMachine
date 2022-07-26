namespace Application.Exceptions
{
    /// <summary>
    /// Exception generated when an entity does not exist in the database
    /// </summary>
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}
