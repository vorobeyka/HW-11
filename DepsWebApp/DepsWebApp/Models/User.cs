namespace DepsWebApp.Models
{
    /// <summary>
    /// User model for authorization
    /// </summary>
    public class User
    {
        /// <summary>
        /// User login
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        public string Password { get; set; }
    }
}
