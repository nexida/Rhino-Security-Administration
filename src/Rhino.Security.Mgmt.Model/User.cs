namespace Rhino.Security.Model
{
    #region Usings

    using Rhino.Security;

    #endregion

    public class User : IUser
    {
        private long id;
        private string name;

        public virtual long Id
        {
            get { return id; }
            set { id = value; }
        }

		[NHibernate.Validator.Constraints.NotNullNotEmpty]
        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }

		private System.Collections.Generic.ICollection<Rhino.Security.Model.UsersGroup> _groups = new System.Collections.Generic.HashSet<Rhino.Security.Model.UsersGroup>();
		public virtual System.Collections.Generic.ICollection<Rhino.Security.Model.UsersGroup> Groups
		{
			get
			{
				return _groups;
			}
			private set
			{
				_groups = value;
			}
		}

        /// <summary>
        /// Gets or sets the security info for this user
        /// </summary>
        /// <value>The security info.</value>
        public virtual SecurityInfo SecurityInfo
        {
            get { return new SecurityInfo(name, id); }
        }

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj)) return true;
			var other = obj as User;
			if (ReferenceEquals(null, other)) return false;
			if (Id != default(System.Int64))
			{
				return other.Id == Id;
			}
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = 0;
				if (Id != default(System.Int64))
				{
					result = (result * 397) ^ Id.GetHashCode();
				}
				else
				{
					result = base.GetHashCode();
				}
				return result;
			}
		}	
    }
}