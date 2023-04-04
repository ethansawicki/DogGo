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
                        n.Id as nId, n.[Name] as nName
                        FROM Owner o
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
                        reader.Close();
                        cmd.CommandText = @"SELECT Id, Name FROM Dog WHERE OwnerId = @dId";
                        cmd.Parameters.AddWithValue("@dId", id);
                        SqlDataReader reader1 = cmd.ExecuteReader();
                            
                         while(reader1.Read())
                            {
                                owner.Doggo.Add(new Doggo()
                                {
                                    Id = reader1.GetInt32(reader1.GetOrdinal("Id")),
                                    Name = reader1.GetString(reader1.GetOrdinal("Name"))
                                });
                            }

                        
                        reader1.Close();
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
