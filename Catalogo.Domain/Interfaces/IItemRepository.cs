using CatalogoApp.Domain.Models;

namespace CatalogoApp.Domain.Interfaces
{
    public interface IItemRepository
    {
        List<Item> ObtenerTodos();
        Item? ObtenerPorId(int id);
        void Agregar(Item item);
        void Eliminar(int id);
    }
}