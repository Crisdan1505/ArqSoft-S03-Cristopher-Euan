using System.Text.Json;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Infrastructure.Repositories
{
    public class JsonItemRepository : IItemRepository
    {
        // Ruta del archivo JSON, relativa a donde corre la app
        private readonly string _filePath;

        public JsonItemRepository(string filePath)
        {
            _filePath = filePath;

            // Si la carpeta no existe, crearla
            var carpeta = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(carpeta))
                Directory.CreateDirectory(carpeta);
        }

        // Lee el JSON y devuelve la lista
        public List<Item> ObtenerTodos()
        {
            if (!File.Exists(_filePath))
                return new List<Item>();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Item>>(json)
                   ?? new List<Item>();
        }

        // Busca un item por Id
        public Item? ObtenerPorId(int id)
        {
            return ObtenerTodos().FirstOrDefault(i => i.Id == id);
        }

        // Agrega un item y guarda la lista completa en el JSON
        public void Agregar(Item item)
        {
            var items = ObtenerTodos();

            // Auto-incrementar el Id
            item.Id = items.Count > 0
                      ? items.Max(i => i.Id) + 1
                      : 1;

            items.Add(item);
            Guardar(items);
        }

        // Elimina por Id y guarda
        public void Eliminar(int id)
        {
            var items = ObtenerTodos();
            var aEliminar = items.FirstOrDefault(i => i.Id == id);
            if (aEliminar != null)
            {
                items.Remove(aEliminar);
                Guardar(items);
            }
        }

        // Método privado: serializa y escribe el archivo
        private void Guardar(List<Item> items)
        {
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true   // JSON legible para humanos
            };
            var json = JsonSerializer.Serialize(items, opciones);
            File.WriteAllText(_filePath, json);
        }
    }
}