using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;
using EmployeeAPI2.Models;
using System.Data;
using System.Threading.Tasks;

namespace EmployeeAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        // Get all employees
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            using var connection = new SqlConnection(_connectionString);
            var employees = await connection.QueryAsync<Employee>(
                "sp_GetAllEmployees",
                commandType: CommandType.StoredProcedure);

            return Ok(employees);
        }

        // Get employee by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var employee = await connection.QueryFirstOrDefaultAsync<Employee>(
                "sp_GetEmployeeById",
                new { EmployeeId = id },
                commandType: CommandType.StoredProcedure);

            return employee != null ? Ok(employee) : NotFound(new { message = "Employee not found." });
        }

        // Add employee
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Employee emp)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.ExecuteAsync(
                "sp_AddEmployee",
                new { emp.EName, emp.Mobile, emp.Email },
                commandType: CommandType.StoredProcedure);

            return Ok(new { message = "Employee added successfully." });
        }

        // Update employee
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Employee emp)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.ExecuteAsync(
                "sp_UpdateEmployee",
                new
                {
                    EmployeeId = id,
                    emp.EName,
                    emp.Mobile,
                    emp.Email
                },
                commandType: CommandType.StoredProcedure);

            return Ok(new { message = "Employee updated successfully." });
        }

        // Delete employee
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.ExecuteAsync(
                "sp_DeleteEmployee",
                new { EmployeeId = id },
                commandType: CommandType.StoredProcedure);

            return Ok(new { message = "Employee deleted successfully." });
        }
    }
}
