using MB.EventResults.Blazor.Shared;
using Microsoft.AspNetCore.Mvc;

namespace MB.EventResults.Blazor.Server;

[ApiController]
public class ImportController : Controller {
  private readonly ILogger<ImportController> _Logger;
  private readonly IAuthService _AuthService;
  private readonly IImportService _ImportService;

  public ImportController(ILogger<ImportController> logger, IAuthService authService, IImportService importService) {
    _Logger = logger;
    _AuthService = authService;
    _ImportService = importService;
  }

  [HttpPost(UrlConstants.Import)]
  [ResponseCache(NoStore = true, Duration = 0)]
  public async Task<IActionResult> Import() {
    if (!_AuthService.CheckAuth(Request.Headers)) {
      return Unauthorized();
    }

    var realFiles = Request.Form.Files.Where(p => p.Length > 0).ToList();

    _Logger.LogInformation($"Processing {realFiles.Count} files.");

    bool any = false;

    foreach (var formFile in realFiles) {
      using (var stream = formFile.OpenReadStream()) {
        any = any || await _ImportService.Import(stream);
      }
    }

    return any ? Ok() : NotFound();
  }

  [HttpGet(UrlConstants.Clear)]
  [ResponseCache(NoStore = true, Duration = 0)]
  public IActionResult Clear() {
    _ImportService.Clear();
    return Ok();
  }

  [HttpGet(UrlConstants.Reindex)]
  [ResponseCache(NoStore = true, Duration = 0)]
  public IActionResult Reindex() {
    _ImportService.Reindex();
    return Ok();
  }
}