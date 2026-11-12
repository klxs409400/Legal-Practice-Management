using LawFirm.Application.Features.References;
using LawFirm.Application.Features.References.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LawFirm.Api.Controllers;

[ApiController]
[Route("api/references")]

public class ReferencesController : ControllerBase
{
    private readonly IReferenceService _referenceService;

    public ReferencesController(IReferenceService referenceService)
    {
        _referenceService = referenceService;
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] ReferenceListRequest request)
    {
        var result = await _referenceService.ListAsync(request);
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = nameof(GetReference))]

    public async Task<IActionResult> GetReference(int id)
    {
        var result = await _referenceService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }



}