using System;
using System.Collections.ObjectModel;
using DatabaseManager;
using Supabase;

public class Gender
{
    public int Id { get; set; }
    public string Name { get; set; }

    private Gender(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    public static async Task<ObservableCollection<Gender>> GetAllGendersEN(Client client)
    {
        List<GenderEntity> dbGenders = await GenderEntity.GetAllGenders(client);
        ObservableCollection<Gender> genders = new();

        foreach (GenderEntity dbGender in dbGenders)
        {
            genders.Add(new Gender(dbGender.Id, dbGender.NameEn));
        }

        return genders;
    }

    public override string ToString()
    {
        return Name;
    }
}
