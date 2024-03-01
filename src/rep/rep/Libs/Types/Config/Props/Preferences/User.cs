namespace rep.Libs.Types.Config.Props.Preferences
{
    /// <summary>
    /// user
    /// </summary>
    internal sealed class User
    {
        #region props

        /// <summary>
        /// first_name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// last_name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// company 
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// project_name
        /// </summary>
        public string ProjectName { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public User()
        {
            FirstName = "example";
            LastName = "example";
            Company = "example";
            ProjectName = "example";
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="firstName">firstName</param>>
        /// <param name="lastName">lastName</param>>
        /// <param name="company">company</param>>
        /// <param name="projectName">project_name</param>>
        public User(string firstName, string lastName, string company, string projectName)
        {
            FirstName = firstName;
            LastName = lastName;
            Company = company;
            ProjectName = projectName;
        }

        #endregion
    }
}
