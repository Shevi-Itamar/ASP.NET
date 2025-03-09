using lessson1.Models;
namespace lessson1.Interfaces
{
    public interface ICartService
    {
        List<Jewel> GetCart(int userId);
        void AddToCart(int userId, int jewelId);
        void RemoveFromCart(int userId, int jewelId);
    }
}

