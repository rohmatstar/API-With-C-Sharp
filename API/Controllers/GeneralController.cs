using API.Contracts;
using API.Models;
using API.Utilities.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;

namespace API.Controllers
{
    public class GeneralController<TEntity> : ControllerBase where TEntity : class
    {
        private readonly IGeneralRepository<TEntity> _repository;

        public GeneralController(IGeneralRepository<TEntity> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var entity = _repository.GetAll();
            if (!entity.Any())
            {
                return NotFound(new ResponseHandler<TEntity>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "No Data Found"
                });
            }

            return Ok(new ResponseHandler<IEnumerable<TEntity>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Found",
                Data = entity
            });
        }


        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var entity = _repository.GetByGuid(guid);
            if (entity is null)
            {
                return NotFound(new ResponseHandler<TEntity>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "No Data Found"
                });
            }

            return Ok(new ResponseHandler<IEnumerable<TEntity>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Found",
                Data = (IEnumerable<TEntity>)entity
            });
        }

        [HttpPost]
        public IActionResult Create(TEntity entity)
        {
            var isCreated = _repository.Create(entity);
            if (isCreated is null)
            {
                return NotFound(new ResponseHandler<TEntity>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Unable to Create Data. Check Your Request"
                });
            }

            return Ok(new ResponseHandler<IEnumerable<TEntity>>
            {
                Code = StatusCodes.Status201Created,
                Status = HttpStatusCode.Created.ToString(),
                Message = "Data Created Successfully"
            });
        }

        [HttpPut]
        public IActionResult Update(TEntity entity)
        {
            var getGuid = (Guid)typeof(TEntity).GetProperty("Guid")!.GetValue(entity)!;
            var isFound = _repository.IsExist(getGuid);

            if (!isFound)
            {
                return NotFound(new ResponseHandler<TEntity>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Invalid ID"
                });
            }

            var isUpdated = _repository.Update(entity);
            if (!isUpdated)
            {
                return NotFound(new ResponseHandler<TEntity>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Unable to Update Data. Check Your Request"
                });
            }

            return Ok(new ResponseHandler<IEnumerable<TEntity>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Updated Successfully",
                Data = (IEnumerable<TEntity>)entity
            });
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var isFound = _repository.IsExist(guid);

            if (!isFound)
            {
                return NotFound(new ResponseHandler<TEntity>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Invalid ID"
                });
            }

            var isDeleted = _repository.Delete(guid);
            if (!isDeleted)
            {
                return NotFound(new ResponseHandler<TEntity>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Unable to Delete Data. Check Your Request"
                });
            }

            return Ok(new ResponseHandler<IEnumerable<TEntity>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Deleted Successfully"
            });
        }
    }
}