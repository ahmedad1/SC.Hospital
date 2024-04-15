﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternWithUOW.Core.DTOs;
using RepositoryPatternWithUOW.Core.Interfaces;
using System.Linq.Expressions;

namespace Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController(IUnitOfWork unitOfWork) : ControllerBase
    {
        [Authorize(Roles ="Adm")]
        [HttpGet("no-verbos")]
        public async Task<IActionResult> GetAllDepartmentsNoVerbos()
        {
            
            return Ok(await unitOfWork.DepartmentRepository.GetAllAsync(false));
        }
        [HttpGet("verbos")]
        public async Task<IActionResult> GetAllDepartmentsVerbos()
        {
            
            return Ok(await unitOfWork.DepartmentRepository.GetAllAsync(true));

        }
        [Authorize(Roles ="Adm")]
        [HttpPost]
        public async Task<IActionResult>Add([FromForm]DepartmentDto deptDto)
        {
           
                await unitOfWork.DepartmentRepository.AddAsync(deptDto);
                await unitOfWork.SaveChangesAsync(); 
                return Ok();
        
        }
        
    }
}
