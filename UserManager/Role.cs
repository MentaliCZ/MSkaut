using System;
using DatabaseManager;
using Supabase;

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

        //**************************************************************************************
        // Database methods
        //**************************************************************************************

        public static async Task<Role?> GetRole(string name, Client client) 
		{
			DBUserRole? dbUserRole = await DBUserRole.GetUserRole(name, client);

			if (dbUserRole == null)
				return null;

			return new Role(dbUserRole.Name, dbUserRole.Description, dbUserRole.Hierarchy);	
		}
    }
}
