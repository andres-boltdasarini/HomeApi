using System.Threading.Tasks;
using AutoMapper;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер комнат
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomRepository _repository;
        private IMapper _mapper;
        
        public RoomsController(IRoomRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        //TODO: Задание - добавить метод на получение всех существующих комнат
        
        /// <summary>
        /// Добавление комнаты
        /// </summary>
        [HttpPost] 
        [Route("")] 
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await _repository.GetRoomByName(request.Name);
            if (existingRoom == null)
            {
                var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                await _repository.AddRoom(newRoom);
                return StatusCode(201, $"Комната {request.Name} добавлена!");
            }
            
            return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        }

        [HttpPut]
        [Route("{name}")]
        public async Task<IActionResult> UpdateRoom(
    [FromRoute] string name,
    [FromBody] EditRoomRequest request)
        {
            var room = await _repository.GetRoomByName(name);
            if (room == null)
                return StatusCode(400, $"Комната {name} не найдена!");

            // Проверка конфликта имен
            if (request.NewName != name)
            {
                var existingRoom = await _repository.GetRoomByName(request.NewName);
                if (existingRoom != null)
                    return StatusCode(409, $"Комната {request.NewName} уже существует!");
            }

            // Обновление свойств
            room.Name = request.NewName;
            room.Area = request.Area;
            room.GasConnected = request.GasConnected;
            room.Voltage = request.Voltage;

            await _repository.UpdateRoom(room, request.NewName);
            return StatusCode(200, $"Комната {name} обновлена! Новое имя: {request.NewName}");
        }
    }
}