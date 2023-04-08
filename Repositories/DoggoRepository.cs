using DogGo.Models;
using DogGo.Utils;
using Microsoft.Data.SqlClient;

namespace DogGo.Repositories
{
    public class DoggoRepository : IDoggoRepository
    {
        private readonly IConfiguration _config;

        public DoggoRepository(IConfiguration config)
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

        public List<Doggo> GetAllDoggos()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], OwnerId, Breed
                        FROM Dog";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Doggo> doggos = new List<Doggo>();
                    while (reader.Read())
                    {
                        Doggo doggo = new Doggo()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                        };

                        doggos.Add(doggo);
                    }
                    reader.Close();
                    return doggos;
                }
            }
        }
        public Doggo GetDoggoById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT Id, [Name], OwnerId, Breed, Notes, ImageUrl
                    FROM Dog
                    WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    Doggo doggo = null;
                    if(reader.Read())
                    {
                        doggo = new Doggo()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = DbUtils.GetString(reader, "Notes"),
                            ImageUrl = DbUtils.GetString(reader, "ImageUrl")
                        };
                    }                     
                    reader.Close();
                    return doggo;
                }
            }
        }
        public void AddDoggo(Doggo doggo)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Dog ([Name], OwnerId, Breed)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @ownerId, @breed)";

                    cmd.Parameters.AddWithValue("@name", doggo.Name);
                    cmd.Parameters.AddWithValue("@ownerId", doggo.OwnerId);
                    cmd.Parameters.AddWithValue("@breed", doggo.Breed);
                    
                    int id = (int)cmd.ExecuteScalar();

                    doggo.Id = id;
                }
            }
        }
        public void UpdateDoggo(Doggo doggo)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    UPDATE Dog
                    SET
                        [Name] = @name,
                        OwnerId = @ownerId,
                        Breed = @breed,
                        Notes = @notes,
                        ImageUrl = NULL
                    WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", doggo.Id);
                    cmd.Parameters.AddWithValue("@name", doggo.Name);
                    cmd.Parameters.AddWithValue("@ownerId", doggo.OwnerId);
                    cmd.Parameters.AddWithValue("@breed", doggo.Breed);
                    cmd.Parameters.AddWithValue("@notes", doggo.Notes);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDoggo(int doggoId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM Dog
                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", doggoId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Doggo> GetDoggosByOwnerId(int ownerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, Name, Breed, Notes, ImageUrl, OwnerId
                        FROM Dog
                        WHERE OwnerId = @ownerId";

                    cmd.Parameters.AddWithValue("@ownerId", ownerId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Doggo> dogs = new List<Doggo>();

                    while (reader.Read())
                    {
                        Doggo doggo = new Doggo()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId"))
                        };

                        if(reader.IsDBNull(reader.GetOrdinal("Notes")) == false)
                        {
                            doggo.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }
                        if (reader.IsDBNull(reader.GetOrdinal("ImageUrl")) == false) 
                        {
                            doggo.ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        }

                        dogs.Add(doggo);
                    }
                        reader.Close();
                        return dogs;
                }
            }
        }
    }
}
