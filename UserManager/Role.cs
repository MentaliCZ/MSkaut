using System;

namespace UserManager
{
	public class Role
	{
		public string Name { get; private set; }
		public string Description { get; private set; }
		// hierarchy, larger number means more privileges
		private int hierarchy;

		public Role(string name, int hierarchy)
		{
			this.Name = name;
			this.hierarchy = hierarchy;
		}

        public Role(string name, string description, int hierarchy) : this(name, hierarchy)
        {
			this.Description = description;
        }
    }
}
