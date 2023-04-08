using DogGo.Models;
using Microsoft.Data.SqlClient;

namespace DogGo.Repositories
{
    public interface IDoggoRepository
    {
        void AddDoggo(Doggo doggo);
        void DeleteDoggo(int doggoId);
        List<Doggo> GetAllDoggos();
        void UpdateDoggo(Doggo doggo);
        Doggo GetDoggoById(int id);
        List<Doggo> GetDoggosByOwnerId(int ownerId);
    }
}