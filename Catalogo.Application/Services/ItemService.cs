using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Application.Services
{
    public class ItemService
    {
        private readonly IItemRepository _repo;

        // El servicio recibe el repositorio por constructor
        // No sabe si es JSON, SQL, memoria, etc.
        public ItemService(IItemRepository repo)
        {
            _repo = repo;
        }

        public List<Item> ObtenerTodos()
        {
            return _repo.ObtenerTodos();
        }

        public Item? ObtenerPorId(int id)
        {
            return _repo.ObtenerPorId(id);
        }

        public void Agregar(Item item)
        {
            // Aquí podrías agregar validaciones de negocio
            // Por ejemplo: if (string.IsNullOrEmpty(item.Titulo)) throw...
            _repo.Agregar(item);
        }

        public void Eliminar(int id)
        {
            _repo.Eliminar(id);
        }

        // Método útil para el filtro por categoría/género
        public List<Item> ObtenerPorGenero(string genero)
        {
            return _repo.ObtenerTodos()
                        .Where(i => i.Genero == genero)
                        .ToList();
        }

        public List<string> ObtenerGeneros()
        {
            return _repo.ObtenerTodos()
                        .Select(i => i.Genero)
                        .Distinct()
                        .ToList();
        }
    }
}