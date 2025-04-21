using System;
using DatabaseManager;
using Supabase;

public class Gender
{
	public int Id { get; set; }
	public string Name { get; set; }

	public Gender(int id, string name)
	{
		this.Id = id;
		this.Name = name;
	}

	public static async Task<Gender> GetGender(int id, Client client)
	{
		DBGender dbGender = await DBGender.GetGender(id, client);

		return new Gender(dbGender.Id, dbGender.NameCs);
	}
}
