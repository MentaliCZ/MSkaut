using System;
using DatabaseManager;
using Supabase;

namespace UserManager
{
	public class Role
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }
		// hierarchy, larger number means more privileges
		private int hierarchy;

		private Role(int id, string name, int hierarchy)
		{
			this.Id = id;
			this.Name = name;
			this.hierarchy = hierarchy;
		}

		private Role(int id, string name, string description, int hierarchy) : this(id, name, hierarchy)
		{
			this.Description = description;
		}

		//**************************************************************************************
		// Database methods
		//**************************************************************************************

		public static async Task<Role?> GetRole(int id, Client client)
		{
			DBUserRole? dbUserRole = await DBUserRole.GetUserRole(id, client);

			if (dbUserRole == null)
				return null;

			return new Role(dbUserRole.Id, dbUserRole.Name, dbUserRole.Description, dbUserRole.Hierarchy);
		}

		public static async Task<bool> CreateRole(string name, string description, int hierarchy, Client client)
		{
			return await DBUserRole.CreateUserRole(name, description, hierarchy, client);
		}
    }
}
