using AutoMapper;
using CRMProject.DTO;
using CRMProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CRMLeadController : ControllerBase
    {
        private readonly TaskDbContext context;
        private readonly IMapper mapper;

        public CRMLeadController(TaskDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<LeadDto>>> GetLeads()
        {
            var leads = await context.Leads.ToListAsync();
            var leadDtos = mapper.Map<List<LeadDto>>(leads);
            return Ok(leadDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeadDto>> GetLeadsById(int id)
        {
            var lead = await context.Leads.FindAsync(id);
            if (lead == null)
            {
                return NotFound();
            }
            var leadDto = mapper.Map<LeadDto>(lead);
            return Ok(leadDto);
        }



        [HttpPost("CreateLeads")]
        public async Task<ActionResult<LeadDto>> CreateLeads(LeadDto leadDto )
        {
            var lead = mapper.Map<Lead>(leadDto); 
            await context.Leads.AddAsync(lead);
            await context.SaveChangesAsync();
            var createdLeadDto = mapper.Map<LeadDto>(lead);
            return Ok(createdLeadDto);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LeadDto>> UpdateLeads(int id, LeadDto leadDto)
        {
            if (id != leadDto.Id)
            {
                return BadRequest();
            }

            var lead = await context.Leads.FindAsync(id);
            if (lead == null)
            {
                return NotFound();
            }

            
            mapper.Map(leadDto, lead);

            context.Entry(lead).State = EntityState.Modified;
            await context.SaveChangesAsync();

            var updatedLeadDto = mapper.Map<LeadDto>(lead); 
            return Ok(updatedLeadDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<LeadDto>> DeleteLeads(int id)
        {
            var lead = await context.Leads.FindAsync(id);
            if (lead == null)
            {
                return NotFound();
            }

            context.Leads.Remove(lead);
            await context.SaveChangesAsync();

            var deletedLeadDto = mapper.Map<LeadDto>(lead); 
            return Ok(deletedLeadDto);
        }

    }
}
