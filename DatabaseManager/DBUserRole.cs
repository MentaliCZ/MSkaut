using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

[Table("User_role")]
public class DBUserRole : BaseModel
{
    [PrimaryKey("user_role_id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("power_int")]
    public int Hierarchy { get; set; }
}
