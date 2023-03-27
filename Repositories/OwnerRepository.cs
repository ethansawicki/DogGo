using DogGo.Models;
using Microsoft.Data.SqlClient;

namespace DogGo.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IConfiguration _config;

        public OwnerRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Owner> GetAllOwners()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT o.Id as oId, o.Email, o.[Name] as oName, o.Address, o.Phone,
                               n.Id as nId, n.Name as nName
                        FROM Owner o
                        JOIN Neighborhood n
                        ON o.NeighborhoodId = n.Id
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Owner> owners = new List<Owner>();
                    while (reader.Read())
                    {
                        Owner owner = new Owner
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("oId")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Name = reader.GetString(reader.GetOrdinal("oName")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone")),
                            Neighborhood = new Neighborhood
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("nId")),
                                Name = reader.GetString(reader.GetOrdinal("nName"))
                            }
                        };

                        owners.Add(owner);
                    }

                    reader.Close();
                    return owners;
                }
            }
        }

        public Owner GetOwnerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT o.Id as oId, o.Email, o.[Name] as oName, o.Address, o.NeighborhoodId, o.Phone,
                        n.Id as nId, n.[Name] as nName,
                        d.id as dId, d.[Name] as dName, d.ImageUrl
                        FROM Owner o
                        JOIN Dog d
                        ON d.OwnerId = o.id
                        JOIN Neighborhood n
                        ON n.id = o.NeighborhoodId
                        WHERE o.Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    Owner owner = null;
                    if (reader.Read())
                    {

                            owner = new Owner
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("oId")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Name = reader.GetString(reader.GetOrdinal("oName")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone")),
                            Neighborhood = new Neighborhood
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("nId")),
                                Name = reader.GetString(reader.GetOrdinal("nName"))
                            },
                            Doggo = new List<Doggo>()
                        };
                         
                            var dogId = reader.GetInt32(reader.GetOrdinal("dId"));
                            var existingDog = owner.Doggo.FirstOrDefault(d => d.Id == dogId);
                            owner.Doggo.Add(new Doggo()
                            {
                                Id = dogId,
                                Name = reader.GetString(reader.GetOrdinal("dName"))
                            });
                        
                        reader.Close();
                        return owner;
                    } else
                    {
                        reader.Close();
                        return null;
                    }
                        
                }
            }
        }
    }
}
